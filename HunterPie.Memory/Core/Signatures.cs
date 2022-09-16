using System;
using System.Collections;
using System.Collections.Generic;

namespace HunterPie.Memory.Core
{
    public class Signatures : ICollection<Signature>
    {
        public List<Signature> PatternList { get; } = new();
        public Signature[] Patterns;
        public readonly int[] FirstBytes = new int[256];
        public int MaximumLength { get; private set; }
        public int PatternsLeft { get; private set; }

        public int Count => PatternList.Count;
        public bool IsReadOnly => false;

        public Signatures Compile()
        {
            Array.Fill(FirstBytes, 0);

            Patterns = PatternList.ToArray();

            foreach (var pattern in Patterns)
                FirstBytes[pattern.Pattern.Bytes[0]]++;

            return this;
        }
        public void Add(Signature item)
        {
            MaximumLength = Math.Max(MaximumLength, item.Pattern.Bytes.Length);
            PatternList.Add(item);
            PatternsLeft++;
        }

        public void Clear() => PatternList.Clear();
        public bool Contains(Signature item) => PatternList.Contains(item);
        public void CopyTo(Signature[] array, int arrayIndex) => PatternList.CopyTo(array, arrayIndex);
        public IEnumerator<Signature> GetEnumerator() => PatternList.GetEnumerator();
        public bool Remove(Signature item) => PatternList.Remove(item);
        IEnumerator IEnumerable.GetEnumerator() => Patterns.GetEnumerator();
        public void Found(Signature signature, long address, long value)
        {
            FirstBytes[signature.Pattern.Bytes[0]]--;
            signature.FoundAt(address, value);
            PatternsLeft--;
        }
    }
}
