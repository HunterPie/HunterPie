using System;

namespace HunterPie.Core.Extensions;

#nullable enable
public static class SyntaxExtensions
{

    public static T Also<T>(this T self, Action<T> block)
    {
        block(self);

        return self;
    }

}
