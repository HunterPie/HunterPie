using HunterPie.Core.Architecture;
using HunterPie.Core.Client;
using HunterPie.Core.Logger;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HunterPie.Core.Domain
{
    public static class ScanManager
    {
        private static Thread thread;
        private static CancellationTokenSource token = new CancellationTokenSource();
        private readonly static HashSet<Scannable> scannables = new HashSet<Scannable>();
        
        // Metrics

        public readonly static Observable<long> ScanTime = 0;
        
        internal static void Start()
        {
            if (thread is null)
            {
                thread = new Thread(async () =>
                {
                    do
                    {
                        try
                        {
                            Stopwatch sw = Stopwatch.StartNew();
                            Scan();
                            sw.Stop();
                            ScanTime.Value = sw.ElapsedMilliseconds;
                            
                            if (token.IsCancellationRequested)
                                break;

                            Thread.Sleep((int)ClientConfig.Config.Client.PollingRate.Current);
                        }
                        catch (Exception err)
                        {
                            // Logs the error if it came from a generic exception instead of a
                            // cancel request
                            Log.Error(err.ToString());
                            continue;
                        }

                    } while (true);

                    token = new();
                })
                {
                    Name = "ScanManager",
                    IsBackground = true,
                    Priority = ThreadPriority.AboveNormal
                };
                thread.Start();
            }
                
        }

        internal static void Stop()
        {
            if (thread is not null)
            {
                scannables.Clear();
                token.Cancel();
                thread = null;
            }
        }

        private static void Scan()
        {

            Scannable[] readOnlyScannables = scannables.ToArray();
            Task[] tasks = new Task[readOnlyScannables.Length];

            for (int i = 0; i < readOnlyScannables.Length; i++)
                tasks[i] = Task.Run(readOnlyScannables[i].Scan);

            Task.WaitAll(tasks);
        }

        public static void Add(params Scannable[] scannableList)
        {
            foreach (Scannable scannable in scannableList)
                Add(scannable);
        }

        public static void Add(Scannable scannable)
        {
            lock (scannables)
            {
                if (scannables.Contains(scannable))
                    return;

                scannables.Add(scannable);
            }
        }

        public static void Remove(Scannable scannable)
        {
            lock (scannables)
            {
                if (!scannables.Contains(scannable))
                    return;

                scannables.Remove(scannable);
            }
        }

    }
}
