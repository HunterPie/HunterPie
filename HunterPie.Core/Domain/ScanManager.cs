using HunterPie.Core.Logger;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace HunterPie.Core.Domain
{
    public static class ScanManager
    {
        private static Thread thread;
        private static CancellationTokenSource token = new CancellationTokenSource();
        private readonly static HashSet<Scannable> scannables = new HashSet<Scannable>();
        
        internal static void Start()
        {
            if (thread is null)
            {
                thread = new Thread(() =>
                {
                    do
                    {
                        try
                        {
                            Scan();

                            lock (token)
                            {
                                if (token.IsCancellationRequested)
                                    break;
                            }
                            Thread.Sleep(30);
                        }
                        catch (Exception err)
                        {
                            // Logs the error if it came from a generic exception instead of a
                            // cancel request
                            Log.Error(err.ToString());
                            continue;
                        }

                    } while (true);
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
                lock (token)
                {
                    token.Cancel();
                }
                thread = null;
            }
        }

        private static void Scan()
        {
            lock (scannables)
            {
                foreach (Scannable scannable in scannables)
                    scannable.Scan();
                    //Task.Factory.StartNew();
            }
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
