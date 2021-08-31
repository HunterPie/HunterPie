using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace HunterPie.Core.Logger
{
    public class Log
    {
        private ILogger io;
        private static Log instance;
        public static Log Instance => instance;

        internal static Log NewInstance(ILogger logger)
        {
            instance = new Log(logger);
            return Instance;
        }

        private Log(ILogger logger)
        {
            io = logger;
        }

        public static Task Debug(params object[] args) => Instance.io.Debug(args);
        public static Task Info(params object[] args) => Instance.io.Info(args);
        public static Task Warn(params object[] args) => Instance.io.Warn(args);
        public static Task Error(params object[] args) => Instance.io.Error(args);
    }
}
