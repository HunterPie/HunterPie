using HunterPie.Core.Architecture;
using HunterPie.Core.Client;
using HunterPie.Core.Domain;
using HunterPie.Core.Observability.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HunterPie.Features.Scan.Service;

internal class ScanService : IControllableScanService, IDisposable
{
    private readonly ILogger _logger = LoggerFactory.Create();
    private Task? _task;
    private readonly HashSet<Scannable> _scannables = new();
    private readonly ReaderWriterLockSlim _lock = new();

    public Observable<long> ElapsedTime { get; } = 0;

    public void Start(CancellationToken cancellationToken)
    {
        _task = Task.Factory.StartNew(
            action: () => ExecuteScanPolling(cancellationToken),
            cancellationToken: cancellationToken,
            creationOptions: TaskCreationOptions.LongRunning,
            scheduler: TaskScheduler.Default
        );
    }

    private void ExecuteScanPolling(CancellationToken cancellationToken)
    {
        while (true)
            try
            {
                var stopWatch = Stopwatch.StartNew();
                Scan(cancellationToken);
                stopWatch.Stop();

                ElapsedTime.Value = stopWatch.ElapsedMilliseconds;

                Thread.Sleep(
                    timeout: TimeSpan.FromMilliseconds(
                            value: ClientConfig.Config.Client.PollingRate.Current
                        )
                    );
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
            }
    }

    private void Scan(CancellationToken cancellationToken)
    {
        try
        {
            _lock.EnterReadLock();

            Task[] tasks = _scannables.Select(async it => await it.ScanAsync())
                .ToArray();

            Task.WaitAll(tasks, cancellationToken);
        }
        finally
        {
            _lock.ExitReadLock();
        }
    }

    public void Add(Scannable scannable)
    {
        try
        {
            _lock.EnterWriteLock();

            if (_scannables.Contains(scannable))
                return;

            _scannables.Add(scannable);
        }
        finally
        {
            _lock.ExitWriteLock();
        }
    }

    public void Remove(Scannable scannable)
    {
        try
        {
            _lock.EnterWriteLock();

            _scannables.Remove(scannable);
        }
        finally
        {
            _lock.ExitWriteLock();
        }
    }

    public void Dispose()
    {
        _lock.Dispose();
        _task?.Dispose();
    }
}