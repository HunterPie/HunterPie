using System.Collections.Generic;

namespace HunterPie.Core.List;

public class SlidingWindow<T>(int size) where T : struct
{
    private int _size = size;
    private readonly LinkedList<T> _list = new();

    public void Add(T value)
    {
        if (_list.Count >= _size)
            _list.RemoveFirst();

        _list.AddLast(value);
    }

    public T? GetFirst() => _list.First?.Value ?? default;

    public T? GetLast() => _list.Last?.Value ?? default;

    public void AdjustSize(int size) => _size = size;
}