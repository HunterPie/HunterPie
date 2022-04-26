using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration;
using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.Core.Game;
using HunterPie.Core.Game.Client;
using HunterPie.Core.System;
using HunterPie.UI.Architecture.Brushes;
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
        private const int MAXIMUM_NUMBER_OF_POINTS = 60;

        private readonly MeterViewModel ViewModel;
        private readonly MeterView View;
        private readonly Context Context;
        private readonly Dictionary<IPartyMember, PlayerViewModel> _members = new();
        private readonly Dictionary<IPartyMember, ChartValues<ObservablePoint>> _playerPoints = new();
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
            foreach (IPartyMember member in Context.Game.Player.Party.Members)
                AddPlayer(member);
        }

        public void HookEvents()
        {
            Context.Game.Player.Party.OnMemberJoin += OnMemberJoin;
            Context.Game.OnTimeElapsedChange += OnTimeElapsedChange;
            Context.Game.Player.OnVillageEnter += OnVillageEnter;
            Context.Game.Player.OnVillageLeave += OnVillageLeave;
        }

        public void UnhookEvents()
        {
            Context.Game.Player.Party.OnMemberJoin -= OnMemberJoin;
            Context.Game.OnTimeElapsedChange -= OnTimeElapsedChange;
            Context.Game.Player.OnVillageEnter -= OnVillageEnter;
            Context.Game.Player.OnVillageLeave -= OnVillageLeave;
            WidgetManager.Unregister<MeterView, DamageMeterWidgetConfig>(View);
        }

        #region Player events
        private void OnVillageLeave(object sender, EventArgs e)
        {
            ViewModel.InHuntingZone = true;
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
                ChartValues<ObservablePoint> points = _playerPoints[member];
                PlayerViewModel vm = _members[member];

                float totalDamage = _members.Keys.Sum(m => m.Damage);
                double newDps = member.Damage / ViewModel.TimeElapsed;
                vm.IsIncreasing = newDps > vm.DPS;
                vm.Percentage = member.Damage / totalDamage * 100;
                vm.DPS = newDps;

                points.Add(new ObservablePoint(ViewModel.TimeElapsed, vm.DPS));

                if (points.Count >= MAXIMUM_NUMBER_OF_POINTS)
                    points.RemoveAt(0);
            }
        }

        private void OnMemberJoin(object sender, IPartyMember e) => View.Dispatcher.Invoke(() => AddPlayer(e));

        private void OnTimeElapsedChange(object sender, IGame e)
        {
            ViewModel.TimeElapsed = e.TimeElapsed;
            if ((int)e.TimeElapsed != lastTimeElapsed)
            {
                View.Dispatcher.Invoke(GetPlayerPoints);
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
        private void AddPlayerSeries(IPartyMember member)
        {
            _playerPoints.Add(member, new());

            ChartValues<ObservablePoint> points = _playerPoints[member];
            
            Color color = (Color)ColorConverter.ConvertFromString("#98ff98");
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

            ViewModel.Series.Add(series);
        }

        private void AddPlayer(IPartyMember member)
        {
            if (_members.ContainsKey(member))
                return;

            _members.Add(member, new(View.Settings) { Name = member.Name, Damage = member.Damage, Weapon = member.Weapon, Color = "#98ff98" });
            member.OnDamageDealt += OnDamageDealt;
            member.OnWeaponChange += OnWeaponChange;

            ViewModel.Players.Add(_members[member]);
            AddPlayerSeries(member);
        }

        private void RemovePlayer(IPartyMember member)
        {
            member.OnDamageDealt -= OnDamageDealt;
            member.OnWeaponChange -= OnWeaponChange;

            ViewModel.Players.Remove(_members[member]);
            _playerPoints.Remove(member);
        }

        #endregion
    }
}
