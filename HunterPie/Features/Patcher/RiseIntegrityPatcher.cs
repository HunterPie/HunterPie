using HunterPie.Core.Address.Map;
using HunterPie.Core.Game;
using HunterPie.Core.Logger;
using System.Linq;

namespace HunterPie.Features.Patcher;

internal static class RiseIntegrityPatcher
{
    /// <summary>
    /// HunterPie has assembly patches for some features to work correctly, patching in-game functions can crash the game
    /// if the integrity check is running. So we need to patch the integrity checker.
    /// If the user has REFramework then it will just skip the patches, since REFramework does that by default, since I 
    /// cannot guarantee an user has it, then HunterPie also patches it if needed.
    /// 
    /// Credits to REFramework: https://github.com/praydog/REFramework
    /// </summary>
    public static void Patch(IContext context)
    {
        long[] crcFuncs =
        {
            AddressMap.GetAbsolute("CRC_FUNC_1"),
            AddressMap.GetAbsolute("CRC_FUNC_2"),
            AddressMap.GetAbsolute("CRC_FUNC_3"),
        };
        byte[][] originalAsms =
        {
            AddressMap.Get<int[]>("CRC_ORIGINAL_FUNC_1")
                .Select(e => (byte)e)
                .ToArray(),

            AddressMap.Get<int[]>("CRC_ORIGINAL_FUNC_2")
                .Select(e => (byte)e)
                .ToArray(),

            AddressMap.Get<int[]>("CRC_ORIGINAL_FUNC_3")
                .Select(e => (byte)e)
                .ToArray(),
        };
        byte[] asmPatch = { 
            // mov al, 0
            0xB0, 0x00, 
            // ret
            0xC3
        };

        for (int i = 0; i < crcFuncs.Length; i++)
        {
            long crcFunc = crcFuncs[i];
            byte[] originalAsm = originalAsms[i];

            byte[] originalInstructions = context.Process.Memory.Read<byte>(crcFunc, 3);

            if (!originalAsm.SequenceEqual(originalInstructions))
                continue;

            context.Process.Memory.InjectAsm(crcFunc, asmPatch);

            Log.Debug("Patched 0x{0:X}", crcFunc);
        }
    }
}
