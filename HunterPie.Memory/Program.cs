using HunterPie.Memory.Core;
using System;
using System.Diagnostics;

namespace HunterPie.Memory
{
    class Program
    {
        static void Main()
        {
            Stopwatch sw = Stopwatch.StartNew();
            Signatures mhrSignatures = new Signatures()
            {
                new("STAGE_ADDRESS", "48 8B 05 ?? ?? ?? ?? 4C 8B 74 24 78 83 78 60 08", 3),
                new("MONSTER_ADDRESS", "48 8B 15 ?? ?? ?? ?? 45 32 ED 4C 8B F8 48 85 D2 74 0B", 3),
                new("LOCKON_ADDRESS", "48 8B 05 ?? ?? ?? ?? 48 89 6C 24 60 8B EE", 3),
                new("CHARACTER_ADDRESS", "48 8B 1D ?? ?? ?? ?? 80 B8 DC 00 00 00 00 0F 85", 3),
                new("SAVE_ADDRESS", "48 8B 15 ?? ?? ?? ?? 45 33 C0 48 8B CE E8 ?? ?? ?? ?? 84 C0 74 1A 48 8B", 3),
                new("WEAPON_ADDRESS", "48 8B 0D ?? ?? ?? ?? 8B 51 70 39 93 84 00 00 00", 3),
                new("SESSION_PLAYERS_ADDRESS", "48 8B 15 ?? ?? ?? ?? 45 33 C0 48 8B CE E8 ?? ?? ?? ?? 84 C0 74 1A 44 0F B6", 3),
                new("ABNORMALITIES_ADDRESS", "48 8B 15 ?? ?? ?? ?? 48 85 D2 74 08 48 8B CB E8 ?? ?? ?? ?? 48 8B 15 ?? ?? ?? ?? 48 85 D2 74 08 48 8B CB E8 ?? ?? ?? ?? 48 8B 15 ?? ?? ?? ?? 48 85 D2 74 0D", 3)
            }.Compile();

            Scanner.Create("MonsterHunterRise")
                .WithSignatures(mhrSignatures)
                .FindProcess()
                .GetMemory()
                .FindSignatures()
                .Results();

            sw.Stop();
            Console.WriteLine("Finished scanning {0}ms", sw.ElapsedMilliseconds);
        }
    }
}
