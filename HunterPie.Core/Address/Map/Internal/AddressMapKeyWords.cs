using System;
using System.Collections.Generic;
using System.Text;

namespace HunterPie.Core.Address.Map.Internal
{
    internal static class AddressMapKeyWords
    {
        public enum AddressMapType
        {
            IntPtr,
            VecInt32,
            Unknown
        }

        private static Dictionary<string, AddressMapType> types = new Dictionary<string, AddressMapType>()
        {
            { "Address", AddressMapType.IntPtr },
            { "Offset", AddressMapType.VecInt32 }
        };

        public static bool IsKeyWord(string word) => types.ContainsKey(word);
        public static AddressMapType GetType(string word) => IsKeyWord(word) ? types[word] : AddressMapType.Unknown;
    }
}
