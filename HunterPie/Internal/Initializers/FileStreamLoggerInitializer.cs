using HunterPie.Core.Observability.Logging;
using HunterPie.Domain.Interfaces;
using HunterPie.Internal.Logger;
using System;
using System.Threading.Tasks;

namespace HunterPie.Internal.Initializers;

internal class FileStreamLoggerInitializer(
    FileStreamLogWriter logWriter
) : IInitializer, IDisposable
{
    public Task Init()
    {
        LoggerFactory.Add(logWriter);

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        logWriter.Dispose();
    }
}