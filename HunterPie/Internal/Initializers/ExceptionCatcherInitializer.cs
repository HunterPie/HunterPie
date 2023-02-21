using HunterPie.Core.Logger;
using HunterPie.Domain.Interfaces;
using HunterPie.Internal.Poogie;
using System;
using System.Threading.Tasks;

namespace HunterPie.Internal.Initializers;

internal class ExceptionCatcherInitializer : IInitializer
{
    public Task Init()
    {
        AppDomain.CurrentDomain.UnhandledException += (_, args) =>
        {
            Log.Error(args.ExceptionObject.ToString());

            RemoteCrashReporter.Send(args.ExceptionObject as Exception);
        };

        return Task.CompletedTask;
    }
}
