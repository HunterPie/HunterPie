using HunterPie.Core.Observability.Logging;
using HunterPie.Domain.Interfaces;
using HunterPie.Features.Input;
using HunterPie.UI.Main.Views;
using System;
using System.Threading.Tasks;
using System.Windows.Interop;

namespace HunterPie.Internal.Initializers;

internal class HotkeyInitializer(
    MainView mainView,
    HotkeyService hotkeyService
) : IInitializer, IDisposable
{
    private readonly ILogger _logger = LoggerFactory.Create();

    public Task Init()
    {
        nint hWnd = new WindowInteropHelper(mainView)
            .EnsureHandle();

        if (HwndSource.FromHwnd(hWnd) is not { } source)
        {
            _logger.Error($"failed to initialize hotkey service");
            return Task.CompletedTask;
        }

        hotkeyService.Setup(source);

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        hotkeyService.Dispose();
    }
}