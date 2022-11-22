using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace HunterPie.Core.Architecture.Collections;
public class ThreadSafeObservableCollection<T> : INotifyCollectionChanged, ICollection<T>
{
    private readonly List<T> _list = new();

    public int Count
    {
        get
        {
            lock (_list)
                return _list.Count;
        }
    }

    public bool IsReadOnly => false;

    public event NotifyCollectionChangedEventHandler CollectionChanged;

    public void Add(T item)
    {
        lock (_list)
            _list.Add(item);

        CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item));
    }

    public void Clear()
    {
        lock (_list)
            _list.Clear();

        CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace));
    }
    public bool Contains(T item)
    {
        lock (_list)
            return _list.Contains(item);
    }
    public void CopyTo(T[] array, int arrayIndex)
    {
        lock (_list)
            _list.CopyTo(array, arrayIndex);
    }

    public IEnumerator<T> GetEnumerator()
    {
        lock (_list)
            return _list.GetEnumerator();
    }

    public bool Remove(T item)
    {
        bool success;

        lock (_list)
            success = _list.Remove(item);

        CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item));
        return success;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        lock (_list)
            return _list.GetEnumerator();
    }
}
