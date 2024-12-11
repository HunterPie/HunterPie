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

    public static TOut? SingleOrNull<TOut>(this IEnumerable<TOut> enumerable)
    {
        IEnumerable<TOut> outs = enumerable as TOut[] ?? enumerable.ToArray();
        int count = outs.Count();

        return count switch
        {
            <= 0 or > 1 => default,
            _ => outs.ElementAtOrDefault(1)
        };
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

    public static IEnumerable<T> TakeRolling<T>(this T[] array, int skip, int take)
    {
        int lastIndex = skip + take;

        if (lastIndex < array.Length)
            return array.Skip(skip)
                .Take(take);

        int takeFromStart = lastIndex - array.Length;

        return array.Take(takeFromStart)
            .Concat(array.Skip(skip)
                .Take(array.Length));
    }

    public static int Count(this IEnumerable enumerable)
    {
        if (enumerable is ICollection collection)
            return collection.Count;

        int count = 0;
        IEnumerator enumerator = enumerable.GetEnumerator();

        while (enumerator.MoveNext())
            count++;

        return count;
    }
}