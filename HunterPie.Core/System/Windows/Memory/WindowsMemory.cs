using HunterPie.Core.Domain.Memory;
using System;

namespace HunterPie.Core.System.Windows.Memory
{
    public class WindowsMemory : IMemory
    {
        IntPtr pHandle;

        public WindowsMemory(IntPtr processHandle)
        {
            pHandle = processHandle;
        }

        public string Read(long address, uint length)
        {
            throw new NotImplementedException();
        }

        public T Read<T>(long address) where T : struct
        {
            throw new NotImplementedException();
        }

        public T[] Read<T>(long address, uint count) where T : struct
        {
            throw new NotImplementedException();
        }

        public long Read(long address, int[] offsets)
        {
            throw new NotImplementedException();
        }

        public bool Write<T>(long address, T data) where T : struct
        {
            throw new NotImplementedException();
        }

        public bool Write<T>(long address, T[] data) where T : struct
        {
            throw new NotImplementedException();
        }
    }
}
