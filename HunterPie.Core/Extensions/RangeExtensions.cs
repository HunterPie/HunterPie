using System;
using System.Collections;
using System.Collections.Generic;
using NotImplementedException = System.NotImplementedException;

namespace HunterPie.Core.Extensions;

public static class RangeExtensions
{
    private struct RangeWrapper : IEnumerator<int>
    {
        private readonly int _end;
        private int _current;

        public RangeWrapper(int start, int end)
        {
            _end = end;
            _current = start;
        }

        public bool MoveNext() => ++_current <= _end;

        public void Reset() => throw new NotImplementedException();

        int IEnumerator<int>.Current => _current;

        object IEnumerator.Current => _current;

        public void Dispose() { }
    }

    public static IEnumerator<int> GetEnumerator(this Range range)
    {
        if (range.Start.IsFromEnd || range.End.IsFromEnd)
            throw new ArgumentException(nameof(range));

        return new RangeWrapper(range.Start.Value, range.End.Value);
    }
}