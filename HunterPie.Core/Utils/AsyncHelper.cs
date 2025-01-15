using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HunterPie.Core.Utils;

public static class AsyncHelper
{

    public static T RunSync<T>(Func<T> asyncDelegate)
    {
        return Task.Run(asyncDelegate)
            .GetAwaiter()
            .GetResult();
    }

    public static async Task<IEnumerable<T>> AwaitAll<T>(this IEnumerable<Task<T>> self)
    {
        return await Task.WhenAll(self);
    }
}