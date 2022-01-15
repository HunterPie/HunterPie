namespace HunterPie.Core.Domain.Memory
{
    public interface IMemoryReader
    {
        public string Read(long address, uint length);
        public T Read<T>(long address) where T : struct;
        public T[] Read<T>(long address, uint count) where T : struct;
        public long ReadPtr(long address, int[] offsets);
        public long Read(long address, int[] offsets);
    }
}
