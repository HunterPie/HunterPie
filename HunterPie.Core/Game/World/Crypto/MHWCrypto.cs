using System;

namespace HunterPie.Core.Game.World.Crypto
{
    public static class MHWCrypto
    {
        const ulong QWORD_MASK = 0xffffffffffffff00;
        const uint DWORD_MASK = 0xffffff00;

        public static float DecryptQuestTimer(ulong value, ulong key)
        {
            byte[] encryptedData = BitConverter.GetBytes(value);

            // First byte
            ulong nextKey = key + key * 2;
            byte tempFirstByte = (byte)((byte)value + (byte)nextKey);
            uint tempMultiplier = (uint)(nextKey & 0xFF);
            uint tempValue = tempMultiplier * 0x3F;
            tempFirstByte ^= (byte)tempValue;
            encryptedData[0] = tempFirstByte;

            // Second byte
            nextKey += 4;
            tempMultiplier = (byte)(nextKey >> 8);
            tempValue = tempMultiplier * 0x3F;
            ulong tempKeyRdx = nextKey >> 8;
            tempKeyRdx = (tempKeyRdx & QWORD_MASK) + (ulong)((byte)tempKeyRdx + encryptedData[1]);
            tempKeyRdx = (tempKeyRdx & QWORD_MASK) + ((byte)tempKeyRdx ^ tempValue);
            encryptedData[1] = (byte)tempKeyRdx;

            // Third byte
            nextKey += 4;
            tempMultiplier = (byte)(nextKey >> 0x10);
            tempValue = tempMultiplier * 0x3F;
            byte tempOriginalValue = encryptedData[2];
            tempOriginalValue += (byte)(nextKey >> (0x8 * 2));
            tempValue = (tempValue & DWORD_MASK) + (uint)((byte)tempValue ^ tempOriginalValue);
            encryptedData[2] = (byte)tempValue;

            // Fourth byte
            nextKey += 4;
            tempMultiplier = (byte)(nextKey >> 0x18);
            tempValue = tempMultiplier * 0x3F;
            tempOriginalValue = encryptedData[3];
            tempOriginalValue += (byte)(nextKey >> (0x8 * 3));
            tempValue ^= tempOriginalValue;
            encryptedData[3] = (byte)tempValue;

            ulong decryptedData = BitConverter.ToUInt64(encryptedData);
            return decryptedData / 60.0f;
        }
    }
}
