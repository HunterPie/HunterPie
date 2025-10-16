﻿using HunterPie.Core.Architecture;
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
            Deaths = 1
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
            IsVisible = true
        });
        viewModel.Players.Add(new(config)
        {
            Name = "Player 2",
            Weapon = Weapon.ChargeBlade,
            Bar = new(config.PlayerSelf),
            IsUser = true,
            IsVisible = true
        });
        viewModel.Players.Add(new(config)
        {
            Name = "Player 3",
            Weapon = Weapon.Greatsword,
            Bar = new(config.PlayerThird),
            IsVisible = true
        });
        viewModel.Players.Add(new(config)
        {
            Name = "Player 4",
            Weapon = Weapon.HuntingHorn,
            Bar = new(config.PlayerFourth),
            IsVisible = true
        });

        LinearSeriesCollectionBuilder builder = new();
        int i = 0;
        foreach (PlayerViewModel? player in viewModel.Players)
        {
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
        int i = 1;
        double newTime = viewModel.TimeElapsed + 0.016;

        if ((int)newTime > (int)viewModel.TimeElapsed)
        {
            viewModel.Pets.TotalDamage = seeder.Next(0, 10_000);
            double maxYAxis = 5;
            foreach (PlayerViewModel player in viewModel.Players)
            {
                ChartValues<ObservablePoint> playerPlots = _playerChartValues[i - 1];

                double lastDps = player.DPS;
                int hit = seeder.Next(0, seeder.Next(1, 20));
                player.Damage += hit;
                player.DPS = player.Damage / viewModel.TimeElapsed;
                player.Bar.Percentage = player.Damage / (double)Math.Max(1, _totalDamage) * 100;
                player.IsIncreasing = lastDps < player.DPS;


                playerPlots.Add(new ObservablePoint(viewModel.TimeElapsed, player.DPS));
                _totalDamage += hit;
                i++;
                maxYAxis = Math.Max(maxYAxis, playerPlots.MaxBy(it => it.Y)?.Y ?? 0);
            }

            viewModel.MaxPlotValue = maxYAxis;
        }
        viewModel.SortMembers();
        viewModel.TimeElapsed = newTime;
    }
}