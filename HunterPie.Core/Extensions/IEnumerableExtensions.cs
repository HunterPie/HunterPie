using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace HunterPie.Core.Extensions;

#nullable enable
public static class IEnumerableExtensions
{

    public static IEnumerable<T> PrependNotNull<T>(this IEnumerable<T> enumerable, T? value) => value is not null ? enumerable.Prepend(value) : enumerable;

    public static void DisposeAll<T>(this T[] enumerable) where T : IDisposable
    {
        foreach (T e in enumerable)
            e.Dispose();
    }

    public static void DisposeAll<T>(this IEnumerable<T> enumerable) where T : IDisposable
    {
        foreach (T e in enumerable)
            e.Dispose();
    }

    public static IEnumerable<TOut> TryCast<TOut>(this IEnumerable enumerable)
    {
        var list = new List<TOut>();

        foreach (object value in enumerable)
            if (value is TOut casted)
                list.Add(casted);

        return list;
    }

    public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> enumerable)
    {
        return new ObservableCollection<T>(enumerable);
    }

    public static IEnumerable<T> FilterNull<T>(this IEnumerable<T?> enumerable) =>
        enumerable.Where(it => it is not null)
            .Cast<T>();

    public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
    {
        foreach (T? item in enumerable)
            action(item);
    }
}
