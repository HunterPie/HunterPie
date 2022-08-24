using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration;
using HunterPie.Core.Client.Configuration.Enums;
using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.Core.Client.Localization;
using HunterPie.Core.Game;
using HunterPie.Core.Game.Client;
using HunterPie.Core.Game.Enums;
using HunterPie.Core.Logger;
using HunterPie.Core.System;
using HunterPie.UI.Architecture.Brushes;
using HunterPie.UI.Overlay.Widgets.Damage.Helpers;
using HunterPie.UI.Overlay.Widgets.Damage.View;
using HunterPie.UI.Overlay.Widgets.Damage.ViewModels;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using ObservableColor = HunterPie.Core.Settings.Types.Color;

namespace HunterPie.UI.Overlay.Widgets.Damage
{
    public class DamageMeterWidgetContextHandler : IContextHandler
    {
        private readonly MeterViewModel ViewModel;
        private readonly MeterView View;
        private readonly Context Context;
        private readonly Dictionary<IPartyMember, MemberInfo> _members = new();
        private readonly Dictionary<IPartyMember, DamageBarViewModel> _pets = new();
        private int lastTimeElapsed = 0;

        public DamageMeterWidgetContextHandler(Context context)
        {
            OverlayConfig config = ClientConfigHelper.GetOverlayConfigFrom(ProcessManager.Game);
            
            View = new MeterView(config.DamageMeterWidget);
            WidgetManager.Register<MeterView, DamageMeterWidgetConfig>(View);

            ViewModel = View.ViewModel;
            Context = context;

            HookEvents();
            UpdateData();
        }

        private void UpdateData()
        {
            ViewModel.Pets.Name = Localization.QueryString("//Strings/Client/Overlay/String[@Id='DAMAGE_METER_OTOMOS_NAME_STRING']");
            ViewModel.InHuntingZone = Context.Game.Player.InHuntingZone;
            ViewModel.Deaths = Context.Game.Deaths;
            ViewModel.TimeElapsed = Context.Game.TimeElapsed;
            
            foreach (IPartyMember member in Context.Game.Player.Party.Members)
                HandleAddMember(member);

            ViewModel.HasPetsToBeDisplayed = _pets.Keys.Count > 0;
        }

        public void HookEvents()
        {
            Context.Game.Player.Party.OnMemberJoin += OnMemberJoin;
            Context.Game.Player.Party.OnMemberLeave += OnMemberLeave;
            Context.Game.OnTimeElapsedChange += OnTimeElapsedChange;
            Context.Game.Player.OnVillageEnter += OnVillageEnter;
            Context.Game.Player.OnVillageLeave += OnVillageLeave;
            Context.Game.OnDeathCountChange += OnDeathCountChange;
        }

        public void UnhookEvents()
        {
            Context.Game.Player.Party.OnMemberJoin -= OnMemberJoin;
            Context.Game.Player.Party.OnMemberLeave -= OnMemberLeave;
            Context.Game.OnTimeElapsedChange -= OnTimeElapsedChange;
            Context.Game.Player.OnVillageEnter -= OnVillageEnter;
            Context.Game.Player.OnVillageLeave -= OnVillageLeave;
            Context.Game.OnDeathCountChange -= OnDeathCountChange;
            WidgetManager.Unregister<MeterView, DamageMeterWidgetConfig>(View);
        }

        #region Player events
        private void OnDeathCountChange(object sender, IGame e)
        {
            ViewModel.Deaths = e.Deaths;
        }

        private void OnVillageLeave(object sender, EventArgs e)
        {
            ViewModel.InHuntingZone = true;

            View.Dispatcher.Invoke(() =>
            {
                foreach (var member in Context.Game.Player.Party.Members)
                    HandleAddMember(member);
            });
        }

        private void OnVillageEnter(object sender, EventArgs e)
        {
            ViewModel.InHuntingZone = false;

            View.Dispatcher.Invoke(() =>
            {
                foreach (var member in _members.Keys)
                    RemovePlayer(member);

                _members.Clear();
            });
        }

        private void GetPlayerPoints()
        {
            foreach (var member in _members.Keys)
            {
                MemberInfo memberInfo = _members[member];
                ChartValues<ObservablePoint> points = (ChartValues<ObservablePoint>)memberInfo.Series.Values;
                PlayerViewModel vm = memberInfo.ViewModel;

                float totalDamage = _members.Keys.Sum(m => m.Damage);
                double newDps = CalculateDpsByConfiguredStrategy(memberInfo);
                vm.IsIncreasing = newDps > vm.DPS;
                vm.Bar.Percentage = totalDamage > 0 ? member.Damage / totalDamage * 100 : 0;
                vm.DPS = newDps;

                var point = CalculatePointByConfiguredStrategy(vm);
                points.Add(point);
            }
        }

        private void CalculatePetsDamage()
        {
            ViewModel.Pets.TotalDamage = _pets.Keys.Sum(pet => pet.Damage);

            foreach (var (pet, vm) in _pets)
                vm.Percentage = pet.Damage / (double)ViewModel.Pets.TotalDamage * 100;

        }

        private void OnMemberJoin(object sender, IPartyMember e) => View.Dispatcher.Invoke(() => HandleAddMember(e));

        private void OnMemberLeave(object sender, IPartyMember e) => View.Dispatcher.Invoke(() => HandleRemoveMember(e));

        private void OnTimeElapsedChange(object sender, IGame e)
        {
            ViewModel.TimeElapsed = e.TimeElapsed;
            if ((int)e.TimeElapsed != lastTimeElapsed)
            {
                View.Dispatcher.Invoke(() =>
                {
                    GetPlayerPoints();
                    CalculatePetsDamage();
                    ViewModel.SortMembers();
                });
                lastTimeElapsed = (int)e.TimeElapsed;
            }
        }

        private void OnDamageDealt(object sender, IPartyMember e)
        {
            MemberInfo member = _members[e];
            PlayerViewModel vm = member.ViewModel;

            if (member.FirstHitAt == -1)
                member.FirstHitAt = ViewModel.TimeElapsed;

            double newDps = CalculateDpsByConfiguredStrategy(member);
            vm.IsIncreasing = newDps > vm.DPS;
            vm.Damage = e.Damage;
            vm.DPS = newDps;
        }
        
        private void OnWeaponChange(object sender, IPartyMember e)
        {
            PlayerViewModel member = _members[e].ViewModel;
            member.Weapon = e.Weapon;
        }
        #endregion

        #region Helpers
        private Series BuildPlayerSeries(string name, string color)
        {
            ChartValues<ObservablePoint> points = new();
            
            Color actualColor = (Color)ColorConverter.ConvertFromString(color);
            var series = new LineSeries()
            {
                Title = name,
                Stroke = new SolidColorBrush(actualColor),
                Fill = ColorFadeGradient.FromColor(actualColor),
                PointGeometrySize = 0,
                StrokeThickness = 2,
                LineSmoothness = 1
            };
            series.Values = points;

            ViewModel.Series.Add(series);
            return series;
        }

        private void HandleAddMember(IPartyMember member)
        {
            Action<IPartyMember>? func = member.Type switch
            {
                MemberType.Player => AddPlayer,
                MemberType.Pet => AddPet,
                _ => null,
            };

            func?.Invoke(member);
        }

        private void HandleRemoveMember(IPartyMember member)
        {
            Action<IPartyMember>? func = member.Type switch
            {
                MemberType.Player => RemovePlayer,
                MemberType.Pet => RemovePet,
                _ => null,
            };

            func?.Invoke(member);
        }

        private void AddPet(IPartyMember pet)
        {
            if (_pets.ContainsKey(pet))
                return;

            ObservableColor playerColor = PlayerConfigHelper.GetColorFromPlayer(
                ProcessManager.Game,
                Math.Max(pet.Slot - 5, 0), 
                pet.IsMyself
            );

            var damageViewModel = new DamageBarViewModel(playerColor);
            ViewModel.Pets.Damages.Add(damageViewModel);
            _pets.Add(pet, damageViewModel);

            ViewModel.HasPetsToBeDisplayed = _pets.Keys.Count > 0;
        }

        private void RemovePet(IPartyMember pet)
        {
            if (!_pets.ContainsKey(pet))
                return;

            _pets.Remove(pet);
            ViewModel.HasPetsToBeDisplayed = _pets.Keys.Count > 0;
        }

        private void AddPlayer(IPartyMember member)
        {
            if (_members.ContainsKey(member))
                return;

            ObservableColor playerColor = PlayerConfigHelper.GetColorFromPlayer(
                ProcessManager.Game,
                member.Slot,
                member.IsMyself
            );

            var memberInfo = new MemberInfo
            {
                ViewModel = new(View.Settings)
                {
                    Name = member.Name,
                    Damage = member.Damage,
                    Weapon = member.Weapon,
                    Bar = new(playerColor),
                    IsUser = member.IsMyself,
                    MasterRank = member.MasterRank
                },
                Series = BuildPlayerSeries(member.Name, playerColor),
                JoinedAt = Context.Game.TimeElapsed
            };

            _members.Add(member, memberInfo);
            
            member.OnDamageDealt += OnDamageDealt;
            member.OnWeaponChange += OnWeaponChange;

            var model = _members[member].ViewModel;

            Log.Debug("Added player: {0:X} {1} with joinedAt: {2}", member.GetHashCode(), member.Name, memberInfo.JoinedAt);

            ViewModel.Players.Add(model);
        }

        private void RemovePlayer(IPartyMember member)
        {
            if (!_members.ContainsKey(member))
                return;

            member.OnDamageDealt -= OnDamageDealt;
            member.OnWeaponChange -= OnWeaponChange;

            ViewModel.Players.Remove(_members[member].ViewModel);
            ViewModel.Series.Remove(_members[member].Series);
            _members.Remove(member);

            Log.Debug("Removed player {0:X}: {1}", member.GetHashCode(), member.Name);
        }

        private double CalculateDpsByConfiguredStrategy(MemberInfo member)
        {
            double timeElapsed = (View.Settings.DpsCalculationStrategy.Value) switch
            {
                DPSCalculationStrategy.RelativeToQuest => ViewModel.TimeElapsed,
                DPSCalculationStrategy.RelativeToJoin => ViewModel.TimeElapsed - Math.Min(ViewModel.TimeElapsed, member.JoinedAt),
                DPSCalculationStrategy.RelativeToFirstHit => ViewModel.TimeElapsed - Math.Min(ViewModel.TimeElapsed, member.FirstHitAt),
                _ => 1,
            };
            timeElapsed = Math.Max(1, timeElapsed);

            return member.ViewModel.Damage / timeElapsed;
        }

        private ObservablePoint CalculatePointByConfiguredStrategy(PlayerViewModel player)
        {
            double damage = (View.Settings.DamagePlotStrategy.Value) switch
            {
                DamagePlotStrategy.TotalDamage => player.Damage,
                DamagePlotStrategy.DamagePerSecond => player.DPS,
                _ => throw new NotImplementedException(),
            };

            return new ObservablePoint(ViewModel.TimeElapsed, damage);
        }
        #endregion
    }
}
