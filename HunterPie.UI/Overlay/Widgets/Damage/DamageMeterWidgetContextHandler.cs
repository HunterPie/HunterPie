using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration;
using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.Core.Game;
using HunterPie.Core.Game.Client;
using HunterPie.Core.Json;
using HunterPie.Core.Logger;
using HunterPie.Core.System;
using HunterPie.UI.Architecture.Brushes;
using HunterPie.UI.Overlay.Widgets.Damage.Helpers;
using HunterPie.UI.Overlay.Widgets.Damage.View;
using HunterPie.UI.Overlay.Widgets.Damage.ViewModel;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

namespace HunterPie.UI.Overlay.Widgets.Damage
{
    public class DamageMeterWidgetContextHandler : IContextHandler
    {
        private readonly MeterViewModel ViewModel;
        private readonly MeterView View;
        private readonly Context Context;
        private readonly Dictionary<IPartyMember, PlayerViewModel> _members = new();
        private readonly Dictionary<IPartyMember, Series> _playerPoints = new();
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
            ViewModel.InHuntingZone = Context.Game.Player.InHuntingZone;
            ViewModel.Deaths = Context.Game.Deaths;
            ViewModel.TimeElapsed = Context.Game.TimeElapsed;

            foreach (IPartyMember member in Context.Game.Player.Party.Members)
                AddPlayer(member);
        }

        public void HookEvents()
        {
            Context.Game.Player.Party.OnMemberJoin += OnMemberJoin;
            Context.Game.OnTimeElapsedChange += OnTimeElapsedChange;
            Context.Game.Player.OnVillageEnter += OnVillageEnter;
            Context.Game.Player.OnVillageLeave += OnVillageLeave;
            Context.Game.OnDeathCountChange += OnDeathCountChange;
        }

        public void UnhookEvents()
        {
            Context.Game.Player.Party.OnMemberJoin -= OnMemberJoin;
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
                    AddPlayer(member);
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
                ChartValues<ObservablePoint> points = (ChartValues<ObservablePoint>)_playerPoints[member].Values;
                PlayerViewModel vm = _members[member];

                float totalDamage = _members.Keys.Sum(m => m.Damage);
                double newDps = member.Damage / ViewModel.TimeElapsed;
                vm.IsIncreasing = newDps > vm.DPS;
                vm.Percentage = totalDamage > 0 ? member.Damage / totalDamage * 100 : 0;
                vm.DPS = newDps;

                if (points.Count >= 50)
                    points.RemoveAt(0);

                points.Add(new ObservablePoint(ViewModel.TimeElapsed, vm.DPS));
            }
        }

        private void OnMemberJoin(object sender, IPartyMember e)
        {
            if (!Context.Game.Player.InHuntingZone)
                return;

            View.Dispatcher.Invoke(() => AddPlayer(e));
        }

        private void OnTimeElapsedChange(object sender, IGame e)
        {
            ViewModel.TimeElapsed = e.TimeElapsed;
            if ((int)e.TimeElapsed != lastTimeElapsed)
            {
                View.Dispatcher.Invoke(() =>
                {
                    GetPlayerPoints();
                    ViewModel.SortPlayers();
                });
                lastTimeElapsed = (int)e.TimeElapsed;
            }
        }

        private void OnDamageDealt(object sender, IPartyMember e)
        {
            PlayerViewModel member = _members[e];

            double newDps = e.Damage / ViewModel.TimeElapsed;
            member.IsIncreasing = newDps > member.DPS;
            member.Damage = e.Damage;
            member.DPS = newDps;
        }
        
        private void OnWeaponChange(object sender, IPartyMember e)
        {
            PlayerViewModel member = _members[e];
            member.Weapon = e.Weapon;
        }
        #endregion

        #region Helpers
        private void AddPlayerSeries(IPartyMember member, PlayerViewModel model)
        {
            ChartValues<ObservablePoint> points = new();
            
            Color color = (Color)ColorConverter.ConvertFromString(model.Color);
            var series = new LineSeries()
            {
                Title = member.Name,
                Stroke = new SolidColorBrush(color),
                Fill = ColorFadeGradient.FromColor(color),
                PointGeometrySize = 0,
                StrokeThickness = 2,
                LineSmoothness = 1
            };
            series.Values = points;

            _playerPoints.Add(member, series);
            ViewModel.Series.Add(series);
        }

        private void AddPlayer(IPartyMember member)
        {
            IPartyMember existingMember = _members.Keys.FirstOrDefault(m => m.Name == member.Name);
            if (existingMember is not null)
            {
                RemovePlayer(existingMember);
                AddPlayer(member);
            }

            if (_members.ContainsKey(member))
                return;

            string playerColor = PlayerConfigHelper.GetColorFromPlayer(
                ProcessManager.Game,
                member.Slot,
                member.IsMyself
            );

            _members.Add(member, new(View.Settings) 
            { 
                Name = member.Name, 
                Damage = member.Damage, 
                Weapon = member.Weapon, 
                Color = playerColor,
                IsUser = member.IsMyself
            });

            member.OnDamageDealt += OnDamageDealt;
            member.OnWeaponChange += OnWeaponChange;

            var model = _members[member];

            Log.Debug("Added player: {0:X} {1}", member.GetHashCode(), member.Name);

            ViewModel.Players.Add(model);
            AddPlayerSeries(member, model);
        }

        private void RemovePlayer(IPartyMember member)
        {
            member.OnDamageDealt -= OnDamageDealt;
            member.OnWeaponChange -= OnWeaponChange;

            ViewModel.Players.Remove(_members[member]);
            ViewModel.Series.Remove(_playerPoints[member]);
            _members.Remove(member);
            _playerPoints.Remove(member);

            Log.Debug("Removed player {0:X}: {1}", member.GetHashCode(), member.Name);
        }

        #endregion
    }
}
