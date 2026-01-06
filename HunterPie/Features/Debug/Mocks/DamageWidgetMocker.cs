using HunterPie.Core.Architecture;
using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.Core.Game.Enums;
using HunterPie.UI.Architecture.Graphs;
using HunterPie.UI.Architecture.Test;
using HunterPie.UI.Overlay.Service;
using HunterPie.UI.Overlay.Views;
using HunterPie.UI.Overlay.Widgets.Damage.ViewModels;
using LiveCharts;
using LiveCharts.Defaults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using System.Windows.Threading;

namespace HunterPie.Features.Debug.Mocks;

internal class DamageWidgetMocker : IWidgetMocker
{
    private int _totalDamage;
    private readonly List<ChartValues<ObservablePoint>> _playerChartValues = new();
    public Observable<bool> Setting => ClientConfig.Config.Development.MockDamageWidget;
    private DispatcherTimer? _timer;
    public WidgetView Mock(IOverlay overlay)
    {
        _timer?.Stop();

        DamageMeterWidgetConfig config = new();
        var viewModel = new MeterViewModelV2(config)
        {
            InHuntingZone = true,
            MaxDeaths = 3,
            Deaths = 1,
        };
        _totalDamage = 0;
        _playerChartValues.Clear();

        CreatePlayers(viewModel, config);
        BindConfiguration(viewModel, config);
        _timer = MockBehavior.Run(() => RunGameSimulation(viewModel, new Random()), 0.016f);
        _timer.Start();

        return overlay.Register(viewModel);
    }

    private void CreatePlayers(MeterViewModelV2 viewModel, DamageMeterWidgetConfig config)
    {
        viewModel.Players.Add(new(config)
        {
            Name = "Player 1",
            Weapon = Weapon.Bow,
            Bar = new(config.PlayerFirst),
            IsVisible = true,
            Affinity = 20,
            RawDamage = 264,
            ElementalDamage = 560,
            MasterRank = 999
        });
        viewModel.Players.Add(new(config)
        {
            Name = "Player 2",
            Weapon = Weapon.ChargeBlade,
            Bar = new(config.PlayerSelf),
            IsUser = true,
            IsVisible = true,
            Affinity = 80,
            RawDamage = 266,
            ElementalDamage = 640,
            MasterRank = 69
        });
        viewModel.Players.Add(new(config)
        {
            Name = "Player 3",
            Weapon = Weapon.Greatsword,
            Bar = new(config.PlayerThird),
            IsVisible = true,
            Affinity = -10,
            RawDamage = 185,
            ElementalDamage = 408,
            MasterRank = 420
        });
        viewModel.Players.Add(new(config)
        {
            Name = "Player 4",
            Weapon = Weapon.HuntingHorn,
            Bar = new(config.PlayerFourth),
            IsVisible = true,
            MasterRank = 111
        });

        LinearSeriesCollectionBuilder builder = new();
        int i = 0;
        foreach (PlayerViewModel player in viewModel.Players)
        {
            PetViewModel petViewModel = CreatePet(player);

            viewModel.Pets.Members.Add(petViewModel);

            _playerChartValues.Add(new ChartValues<ObservablePoint>());
            var color = (Color)ColorConverter.ConvertFromString(player.Bar.Color);
            builder.AddSeries(
                points: _playerChartValues[i],
                title: player.Name,
                color: color
            );
            i++;
        }

        viewModel.Series.AddRange(builder.Build());
    }

    private PetViewModel CreatePet(PlayerViewModel player)
    {
        return new PetViewModel(new DamageBarViewModel(player.Bar.Color))
        {
            Name = player.Name,
        };
    }

    private void BindConfiguration(
        MeterViewModelV2 viewModel,
        DamageMeterWidgetConfig config)
    {
        config.ShowOnlySelf.PropertyChanged += (_, __) =>
        {
            bool shouldHideOthers = config.ShowOnlySelf.Value;
            viewModel.UIThread.BeginInvoke(() =>
            {
                for (int i = 0; i < _playerChartValues.Count; i++)
                {
                    PlayerViewModel member = viewModel.Players[i];

                    member.IsVisible = member.IsUser || !shouldHideOthers;
                }
            });
        };
    }

    private void RunGameSimulation(
        MeterViewModelV2 viewModel,
        Random seeder)
    {
        viewModel.HasPetsToBeDisplayed = true;

        int i = 1;
        double newTime = viewModel.TimeElapsed + 0.016;

        if ((int)newTime > (int)viewModel.TimeElapsed)
        {
            double maxYAxis = 5;

            double[] nextDamages = new double[4] { 300, 200, 100, 50 }
            .Select((dmg, index) =>
            {
                const double variance = 0.1;
                double multiplier = ((seeder.NextDouble() * 2) - 1) * variance;
                double critMultiplier = seeder.NextDouble() > 0.85
                    ? 1 + ((index + 1.0) / 4)
                    : 1.0;
                return dmg * (1 + multiplier) * critMultiplier;
            }).ToArray();

            _totalDamage += (int)nextDamages.Sum();

            foreach (PlayerViewModel player in viewModel.Players)
            {
                ChartValues<ObservablePoint> playerPlots = _playerChartValues[i - 1];

                double lastDps = player.DPS;
                player.Damage += (int)nextDamages[i - 1];
                player.DPS = player.Damage / viewModel.TimeElapsed;
                player.Bar.Percentage = player.Damage / (double)Math.Max(1, _totalDamage) * 100;
                player.IsIncreasing = lastDps < player.DPS;


                playerPlots.Add(new ObservablePoint(viewModel.TimeElapsed, player.DPS));

                i++;
                maxYAxis = Math.Max(maxYAxis, playerPlots.MaxBy(it => it.Y)?.Y ?? 0);
            }

            viewModel.MaxPlotValue = maxYAxis;

            double lastTotalDamage = viewModel.Pets.TotalDamage;

            viewModel.Pets.TotalDamage += (int)(nextDamages.Sum() / 10.0);
            double totalDamage = viewModel.Pets.TotalDamage;

            for (int j = 0; j < nextDamages.Length; j++)
            {
                DamageBarViewModel petVm = viewModel.Pets.Members[j].DamageBar;

                double damage = nextDamages[j] / 10.0;
                double lastDamage = lastTotalDamage * (petVm.Percentage / 100);

                petVm.Percentage = (damage + lastDamage) / totalDamage * 100;
            }

            viewModel.SortMembers();
        }

        viewModel.TimeElapsed = newTime;
    }
}