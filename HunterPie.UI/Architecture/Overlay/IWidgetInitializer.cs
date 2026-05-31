using HunterPie.Core.Domain.Enums;
using System;
using System.Threading.Tasks;

namespace HunterPie.UI.Architecture.Overlay;

internal interface IWidgetInitializer : IDisposable
{
    public GameProcessType SupportedGames { get; }

    public Task LoadAsync();
}