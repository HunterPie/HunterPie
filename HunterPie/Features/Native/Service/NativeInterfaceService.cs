using HunterPie.Core.Client;
using HunterPie.Core.Game;
using HunterPie.Core.Native.IPC;
using HunterPie.Core.Observability.Logging;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace HunterPie.Features.Native.Service;

internal class NativeInterfaceService(
    IContext context
)
{
    private readonly ILogger _logger = LoggerFactory.Create();

    private const string Name = "HunterPie.Native.dll";
    private const string Path = "libs/" + Name;
    private const int Retry = 10;

    public async Task<bool> ConnectAsync()
    {
        bool isInjected = await InjectAsync();

        if (!isInjected)
            return false;

        return await WaitForConnectionAsync();
    }

    private async Task<bool> InjectAsync()
    {
        try
        {
            string native = ClientInfo.GetPathFor(Path);

            if (IsInjected())
            {
                _logger.Debug("HunterPie Native Interface is already running");
                return false;
            }

            await context.Process.Memory.InjectAsync(native);

            _logger.Info("HunterPie Native Interface injected successfully");
            return true;
        }
        catch (Exception ex)
        {
            _logger.Error($"Failed to inject HunterPie Native Interface: {ex}");
            return false;
        }
    }

    private async Task<bool> WaitForConnectionAsync()
    {
        for (int i = 0; i < Retry; i++)
        {
            _logger.Debug($"Trying to connect: Attempt {i}...");

            bool isConnected = await IPCService.Initialize();

            if (isConnected)
                return true;

            await Task.Delay((i + 1) * 100);
        }

        return false;
    }

    private bool IsInjected()
    {
        return context.Process.SystemProcess.Modules
            .Cast<ProcessModule>()
            .Any(it => it.ModuleName == Name);
    }
}