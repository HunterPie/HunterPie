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
        private readonly Dictionary<IPartyMember, ChartValues<ObservablePoint>> _playerPoints = new();
        private float lastTimeElapsed = 0;

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
            foreach (IPartyMember member in Context.Game.Player.Party.Members)
            {
                _members.Add(member, new(View.Settings) { Name = member.Name, Damage = member.Damage, Weapon = member.Weapon, Color = "#98ff98" });
                member.OnDamageDealt += OnDamageDealt;
                member.OnWeaponChange += OnWeaponChange;

                ViewModel.Players.Add(_members[member]);
                AddPlayerSeries(member);
            }
        }

        public void HookEvents()
        {
            Context.Game.Player.Party.OnMemberJoin += OnMemberJoin;
            Context.Game.OnTimeElapsedChange += OnTimeElapsedChange;
        }

        public void UnhookEvents()
        {
            Context.Game.Player.Party.OnMemberJoin -= OnMemberJoin;
            Context.Game.OnTimeElapsedChange -= OnTimeElapsedChange;
            WidgetManager.Unregister<MeterView, DamageMeterWidgetConfig>(View);
        }

        #region Player events
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
            }
        }

        private void OnMemberJoin(object sender, IPartyMember e)
        {
            _members.Add(e, new(View.Settings) { Name = e.Name, Damage = e.Damage, Weapon = e.Weapon, Color = "#98ff98" });
            e.OnDamageDealt += OnDamageDealt;
            e.OnWeaponChange += OnWeaponChange;
            AddPlayerSeries(e);
        }

        private void OnTimeElapsedChange(object sender, IGame e)
        {
            ViewModel.TimeElapsed = e.TimeElapsed;
            if (e.TimeElapsed - lastTimeElapsed > 300)
            {
                GetPlayerPoints();
                lastTimeElapsed = e.TimeElapsed;
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
                LineSmoothness = 0.7
            };
            series.Values = points;

            ViewModel.Series.Add(series);
        }
    }
}
