namespace HunterPie.Memory.Core
{
    public class Signature
    {
        public string Name { get; }
        public Pattern Pattern;
        public long AtAddress { get; private set; }
        public long Value { get; private set; }
        public long Offset { get; }
        public bool IsRelative { get; }
        public bool HasBeenFound { get; private set; }

        public Signature(string name, string pattern, long offset = 0, bool isRelative = true)
        {
            Name = name;
            Pattern = new Pattern(pattern);
            Offset = offset;
            IsRelative = isRelative;
        }

        public void FoundAt(long address, long value)
        {
            HasBeenFound = true;
            AtAddress = address;
            Value = value;
        }
    }
}
