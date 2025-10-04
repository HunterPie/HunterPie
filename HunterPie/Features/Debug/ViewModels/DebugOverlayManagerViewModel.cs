using HunterPie.Core.Architecture;
using HunterPie.Features.Overlay.Services;
using HunterPie.UI.Architecture;
using HunterPie.UI.Settings.Converter.Model;
using HunterPie.UI.Settings.ViewModels.Internal;
using System;

namespace HunterPie.Features.Debug.ViewModels;

internal class DebugOverlayManagerViewModel : ViewModel
{
    public BooleanPropertyViewModel IsDesignModeEnabled { get; }
    public BooleanPropertyViewModel IsGameFocused { get; }
    public BooleanPropertyViewModel IsGameHudOpen { get; }

    public DebugOverlayManagerViewModel(OverlayManager manager)
    {
        IsDesignModeEnabled = CreateBooleanObservable(
            name: "Is design mode enabled",
            description: "Simulates design mode",
            callback: state => manager.IsDesignModeEnabled = state
        );
        IsGameFocused = CreateBooleanObservable(
            name: "Is game focused",
            description: "Simulates game window focus",
            callback: state => manager.IsGameFocused = state
        );
        IsGameHudOpen = CreateBooleanObservable(
            name: "Is Hud open",
            description: "Simulates game Hud state",
            callback: state => manager.IsGameHudVisible = state
        );
    }

    private static BooleanPropertyViewModel CreateBooleanObservable(
        string name,
        string description,
        Action<bool> callback)
    {
        Observable<bool> observable = new(false);
        observable.PropertyChanged += (_, __) => callback(observable.Value);

        return new BooleanPropertyViewModel(observable)
        {
            Name = name,
            Description = description,
            Group = "",
            RequiresRestart = false,
            Conditions = Array.Empty<PropertyCondition>(),
            IsMatch = true
        };
    }
}