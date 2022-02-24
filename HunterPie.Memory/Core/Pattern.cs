using System;
using System.Globalization;

namespace HunterPie.Memory.Core
{
    public class Pattern : IEquatable<byte[]>
    {
        public byte[] Bytes;

        public Pattern(string pattern)
        {
            Bytes = ConvertPatternWithMask(pattern);
        }

        private static byte[] ConvertPatternWithMask(string pattern)
        {
            string[] bytes = pattern.Split(" ");
            Span<byte> converted = bytes.Length <= 256 
                ? stackalloc byte[bytes.Length]
                : new byte[bytes.Length];

            for (int i = 0; i < bytes.Length; i++)
            {
                ref string rawValue = ref bytes[i];

                if (rawValue.StartsWith("?"))
                {
                    converted[i] = 0xFF;
                    continue;
                }

                bool success = byte.TryParse(rawValue, NumberStyles.HexNumber, null, out byte result);
                converted[i] = result;
            }

            return converted.ToArray();
        }

        public bool Equals(byte[] other)
        {
            if (other.Length < Bytes.Length)
                return false;

            for (int i = 0; i < Bytes.Length; i++)
            {
                byte pattern = Bytes[i];
                
                if (pattern == 0xFF)
                    continue;
                
                byte comparedTo = other[i];

                if (pattern != comparedTo)
                    return false;
            }

            return true;
        }
    }
}
