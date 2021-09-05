using System.Collections.Generic;
using System.Threading;

namespace HunterPie.Core.Logger
{
    public class Log
    {
        private readonly List<ILogger> _io = new List<ILogger>();
        private static Log _instance;
        private readonly static SemaphoreSlim _semaphore = new(1, 1);
        public static Log Instance
        {
            get {
                if (_instance is null)
                {
                    _instance = new Log();
                }

                return _instance;
            }
        }

        public static async void Add(ILogger logger)
        {

            try
            {
                await _semaphore.WaitAsync();
                Instance._io.Add(logger);
            }
            catch { }
            finally
            {
                _semaphore.Release();
            }
        }

        public static async void Debug(params object[] args)
        {
            try
            {
                await _semaphore.WaitAsync();

                foreach (ILogger logger in Instance._io)
                    await logger.Debug(args);

            }
            catch { }
            finally
            {
                _semaphore.Release();
            }
        }

        public static async void Info(params object[] args)
        {
            try
            {
                await _semaphore.WaitAsync();

                foreach (ILogger logger in Instance._io)
                    await logger.Info(args);

            }
            catch { }
            finally
            {
                _semaphore.Release();
            }
        }
        public static async void Warn(params object[] args)
        {
            try
            {
                await _semaphore.WaitAsync();

                foreach (ILogger logger in Instance._io)
                    await logger.Warn(args);

            }
            catch { }
            finally
            {
                _semaphore.Release();
            }
        }

        public static async void Error(params object[] args)
        {
            try
            {
                await _semaphore.WaitAsync();

                foreach (ILogger logger in Instance._io)
                    await logger.Error(args);

            } 
            catch { }
            finally
            {
                _semaphore.Release();
            }
        }

    }
}
