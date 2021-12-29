using System.Threading.Tasks;

namespace HunterPie.Core.Logger
{
    public interface ILogger
    {
        public Task Info(params object[] args);
        public Task Warn(params object[] args);
        public Task Error(params object[] args);
        public Task Debug(params object[] args);
        public Task Benchmark(params object[] args);
    }
}
