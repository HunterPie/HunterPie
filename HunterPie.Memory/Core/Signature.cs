namespace HunterPie.Memory.Core
{
    public class Signature
    {
        public string Name { get; }
        public Pattern Pattern;
        public long AtAddress { get; private set; }
        public long Value { get; private set; }
        public long Offset { get; private set; }
        public bool HasBeenFound { get; private set; }

        public Signature(string name, string pattern, long offset = 0)
        {
            Name = name;
            Pattern = new Pattern(pattern);
            Offset = offset;
        }

        public void FoundAt(long address, long value)
        {
            HasBeenFound = true;
            AtAddress = address;
            Value = value;
        }
    }
}
