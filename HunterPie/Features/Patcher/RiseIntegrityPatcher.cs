﻿using HunterPie.Core.Address.Map;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game;
using HunterPie.Core.Logger;
using HunterPie.Core.System.Windows.Native;
using System;
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

            if (crcFunc.IsNullPointer())
                continue;

            byte[] originalAsm = originalAsms[i];

            byte[] originalInstructions = context.Process.Memory.Read<byte>(crcFunc, 3);

            if (!originalAsm.SequenceEqual(originalInstructions))
                continue;

            context.Process.Memory.InjectAsm(crcFunc, asmPatch);

            Log.Debug("Patched 0x{0:X}", crcFunc);
        }
    }

    /// <summary>
    /// Starting with v16.0.2.0 MHRise now hooks NtProtectVirtualMemory, so we gotta patch that for HunterPie.Native to work
    /// </summary>
    public static void PatchProtectVirtualMemory(IContext context)
    {
        // TODO: Make this platform agnostic
        IntPtr ntdllAddress = Kernel32.GetModuleHandle("ntdll");

        if (ntdllAddress == IntPtr.Zero)
        {
            Log.Error("Failed to find ntdll address");
            return;
        }

        Log.Debug("Found ntdll address at {0:X}", ntdllAddress);

        IntPtr ntProtectVirtualMemory = Kernel32.GetProcAddress(ntdllAddress, "NtProtectVirtualMemory");

        if (ntProtectVirtualMemory == IntPtr.Zero)
        {
            Log.Error("Failed to find ntdll::NtProtectVirtualMemory address");
            return;
        }

        Log.Debug("Found ntdll::NtProtectVirtualMemory address at {0:X}", ntProtectVirtualMemory);

        byte[] originalBytes = { 0x4C, 0x8B, 0xD1, 0xB8, 0x50, 0x00, 0x00, 0x00 };
        context.Process.Memory.InjectAsm((long)ntProtectVirtualMemory, originalBytes);
    }
}