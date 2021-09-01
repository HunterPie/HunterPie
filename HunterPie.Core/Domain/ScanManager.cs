using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace HunterPie.Core.Domain
{
    public static class ScanManager
    {
        private static CancellationTokenSource token = new CancellationTokenSource();
        private readonly static HashSet<Scannable> scannables = new HashSet<Scannable>();
        
        internal static void Start()
        {
            if (token.IsCancellationRequested)
            {
                token.Dispose();
                token = new CancellationTokenSource();

                _ = new Task(async () =>
                {
                    do
                    {
                        try
                        {
                            Scan();

                            await Task.Delay(50);

                            token.Token.ThrowIfCancellationRequested();

                        }
                        catch { return; }

                    } while (true);
                }, token.Token, TaskCreationOptions.LongRunning);
            }
                
        }

        internal static void Stop() => token.Cancel();

        private static void Scan()
        {
            lock (scannables)
            {
                foreach (Scannable scannable in scannables)
                    Task.Factory.StartNew(scannable.Scan);
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
