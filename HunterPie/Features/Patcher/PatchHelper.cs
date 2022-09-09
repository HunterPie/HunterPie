﻿using System;
using System.Linq;
using HunterPie.Core.Address.Map;
using HunterPie.Core.Domain.Memory;

namespace HunterPie.Features.Patcher;

internal static class PatchHelper
{
    /// <inheritdoc cref="CheckMemory(IMemory,long,string)"/>
    /// <param name="memoryLocationName">the name of an Address existing in <see cref="AddressMap"/>.</param>
    public static bool CheckMemory(IMemory memory, string memoryLocationName, string expectedDataName)
    {
        var memoryLocation = AddressMap.GetAbsolute(memoryLocationName);
        return CheckMemory(memory, memoryLocation, expectedDataName);
    }

    /// <summary>
    /// Checks whether the process memory sequence starting from the specified memory address
    /// has the same data.
    /// </summary>
    /// <param name="memory">the memory to inspect.</param>
    /// <param name="memoryLocation">starting memory address.</param>
    /// <param name="expectedDataName">the name of a byte sequence (Instruction) existing in <see cref="AddressMap"/>.</param>
    public static bool CheckMemory(IMemory memory, long memoryLocation, string expectedDataName)
    {
        // TODO AddressMap.Get should support returning byte[] directly.
        var expectedData = AddressMap.Get<int[]>(expectedDataName)
            .Select(b => (byte)b)
            .ToArray();
        if (expectedData.Length is < 1 or > 32)
        {
            // This is only a safeguard against blatant errors in the address map.
            throw new InvalidOperationException($"Unexpected data length: {expectedData.Length}.");
        }
        var actualData = memory.Read<byte>(memoryLocation, (uint)expectedData.Length);
        return actualData.SequenceEqual(expectedData);
    }
}