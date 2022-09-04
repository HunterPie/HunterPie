using System;
using HunterPie.Core.Game;
using HunterPie.Core.Address.Map;
using HunterPie.Core.Logger;

namespace HunterPie.Features.Patcher;

internal static class WorldIntegrityPatcher
{

    public static void Patch(Context context)
    {
        var crc1a = AddressMap.GetAbsolute("CRC_FUNC_1A");
        var crc1b = AddressMap.GetAbsolute("CRC_FUNC_1B");
        if (PatchHelper.CheckMemory(context.Process.Memory, crc1a, "CRC_ORIGINAL_FUNC_1A")
            && PatchHelper.CheckMemory(context.Process.Memory, crc1b, "CRC_ORIGINAL_FUNC_1B"))
        {
            const int JMP_INSTRUCTION_LENGTH = 1 + 4;
            var patch = new byte[JMP_INSTRUCTION_LENGTH + 1];
            // JMP rel32: Jump near, relative, RIP = RIP + 32-bit displacement sign extended to 64-bits
            patch[0] = 0xE9;
            var rel32 = (int)(crc1b - crc1a - JMP_INSTRUCTION_LENGTH);
            var written = BitConverter.TryWriteBytes(patch.AsSpan(1, 4), rel32);
            System.Diagnostics.Debug.Assert(written);
            // NOP
            patch[JMP_INSTRUCTION_LENGTH + 0] = 0x90;
            context.Process.Memory.InjectAsm(crc1a, patch);
            Log.Debug("Patched 0x{0:X}", crc1a);
        }
    }

}
