using System;
using System.Collections;
using System.Collections.Generic;

namespace HunterPie.Memory.Core
{
    public class Signatures : ICollection<Signature>
    {
        public List<Signature> _patterns { get; } = new();
        public Signature[] Patterns;
        public readonly int[] FirstBytes = new int[256];
        public int MaximumLength { get; private set; }
        public int PatternsLeft { get; private set; }

        public int Count => _patterns.Count;
        public bool IsReadOnly => false;

        public Signatures Compile()
        {
            Array.Fill(FirstBytes, 0);

            Patterns = _patterns.ToArray();

            foreach (var pattern in Patterns)
                FirstBytes[pattern.Pattern.Bytes[0]]++;

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
        IEnumerator IEnumerable.GetEnumerator() => Patterns.GetEnumerator();
        public void Found(Signature signature, long address, long value)
        {
            FirstBytes[signature.Pattern.Bytes[0]]--;
            signature.FoundAt(address, value);
            PatternsLeft--;
        }
    }
}
