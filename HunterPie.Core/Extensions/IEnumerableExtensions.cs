using System.Collections.Generic;
using System.Linq;

namespace HunterPie.Core.Extensions;
public static class IEnumerableExtensions
{

    public static IEnumerable<T> PrependNotNull<T>(this IEnumerable<T> enumerable, T? value) => value is not null ? enumerable.Prepend(value) : enumerable;

}
