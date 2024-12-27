using HunterPie.UI.Architecture;
using System;

namespace HunterPie.UI.Main.Navigators.Events;

internal class NavigationRequestEventArgs : EventArgs
{
    public required ViewModel ViewModel { get; init; }
}