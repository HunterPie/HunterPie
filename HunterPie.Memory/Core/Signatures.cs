using System;
using System.Collections;
using System.Collections.Generic;

namespace HunterPie.Memory.Core
{
    public class Signatures : ICollection<Signature>
    {
        public List<Signature> _patterns { get; } = new();
        public Signature[] Patterns;
        public int MaximumLength { get; private set; }
        public int PatternsLeft { get; private set; }

        public int Count => _patterns.Count;
        public bool IsReadOnly => false;
        public Signatures Compile()
        {
            Patterns = _patterns.ToArray();
            return this;
        }
        public void Add(Signature item)
        {
            MaximumLength = Math.Max(MaximumLength, item.Pattern.Bytes.Length);
            _patterns.Add(item);
            PatternsLeft++;
        }
        public void Clear() => _patterns.Clear();
        public bool Contains(Signature item) => _patterns.Contains(item);
        public void CopyTo(Signature[] array, int arrayIndex) => _patterns.CopyTo(array, arrayIndex);
        public IEnumerator<Signature> GetEnumerator() => _patterns.GetEnumerator();
        public bool Remove(Signature item) => _patterns.Remove(item);
        IEnumerator IEnumerable.GetEnumerator() => _patterns.GetEnumerator();
        public void Found(Signature signature, long address, long value)
        {
            signature.FoundAt(address, value);
            PatternsLeft--;
        }
    }
}
