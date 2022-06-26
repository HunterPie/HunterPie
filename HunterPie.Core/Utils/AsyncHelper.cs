using System;
using System.Threading.Tasks;

namespace HunterPie.Core.Utils
{
    public static class AsyncHelper
    {

        public static T RunSync<T>(Func<T> asyncDelegate)
        {
            return Task.Run(asyncDelegate)
                .GetAwaiter()
                .GetResult();
        }

    }
}
