﻿using HunterPie.Core.Architecture;
using HunterPie.Core.Game.Entity.Environment;
using HunterPie.Core.Game.Enums;

namespace HunterPie.UI.Overlay.Widgets.Activities.ViewModel;

public class SteamFuelViewModel : Bindable, IActivity
{
    private int _naturalFuel;
    public int NaturalFuel { get => _naturalFuel; set => SetValue(ref _naturalFuel, value); }

    private int _maxNaturalFuel;
    public int MaxNaturalFuel { get => _maxNaturalFuel; set => SetValue(ref _maxNaturalFuel, value); }

    private int _storedFuel;
    public int StoredFuel { get => _storedFuel; set => SetValue(ref _storedFuel, value); }

    public ActivityType Type => ActivityType.Steamworks;
}