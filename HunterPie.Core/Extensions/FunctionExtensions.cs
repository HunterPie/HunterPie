using System;
using System.Threading;
using System.Threading.Tasks;

namespace HunterPie.Core.Extensions
{
    public static class FunctionExtensions
    {
        public static Action<T> Debounce<T>(this Action<T> func, int milliseconds = 300)
        {
            var last = 0;
            return (T param) =>
            {
                var current = Interlocked.Increment(ref last);
                Task.Delay(milliseconds).ContinueWith(task =>
                {
                    if (current == last) 
                        func(param);

                    task.Dispose();
                });
            };
        }
    }
}
