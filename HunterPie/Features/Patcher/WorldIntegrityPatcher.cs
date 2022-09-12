using System;
using HunterPie.Core.Game;
using HunterPie.Core.Address.Map;
using HunterPie.Core.Logger;

namespace HunterPie.Features.Patcher;

internal static class WorldIntegrityPatcher
{

    public static void Patch(Context context)
    {
        PatchCrc("CRC_FUNC_1A", "CRC_FUNC_1B", "CRC_ORIGINAL_FUNC_1A", "CRC_ORIGINAL_FUNC_1B");
        PatchCrc("CRC_FUNC_2A", "CRC_FUNC_2B", "CRC_ORIGINAL_FUNC_2A", "CRC_ORIGINAL_FUNC_2B");

        void PatchCrc(string addressName1, string addressName2, string originalFuncName1, string originalFuncName2)
        {
            var crc1 = AddressMap.GetAbsolute(addressName1);
            var crc2 = AddressMap.GetAbsolute(addressName2);
            if (PatchHelper.CheckMemory(context.Process.Memory, crc1, originalFuncName1)
                && PatchHelper.CheckMemory(context.Process.Memory, crc2, originalFuncName2))
            {
                const int JMP_INSTRUCTION_LENGTH = 1 + 4;
                var patch = new byte[JMP_INSTRUCTION_LENGTH + 1];
                // JMP rel32: Jump near, relative, RIP = RIP + 32-bit displacement sign extended to 64-bits
                patch[0] = 0xE9;
                var rel32 = (int)(crc2 - crc1 - JMP_INSTRUCTION_LENGTH);
                var written = BitConverter.TryWriteBytes(patch.AsSpan(1, 4), rel32);
                System.Diagnostics.Debug.Assert(written);
                // NOP
                patch[JMP_INSTRUCTION_LENGTH + 0] = 0x90;
                context.Process.Memory.InjectAsm(crc1, patch);
                Log.Debug("Patched 0x{0:X}", crc1);
            }
        }
    }

}
