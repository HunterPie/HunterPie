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
                new("STAGE_ADDRESS", "48 8B 05 ?? ?? ?? ?? ?? 8B ?? ?? ?? ?? 83 78 60 ?? 74 ??", 3),
                new("MONSTER_ADDRESS", "48 8B 05 ?? ?? ?? ?? 48 8B 88 ?? ?? ?? ?? 4A 8B 44 C9 20", 3),
                new("LOCKON_ADDRESS", "48 8B 05 ?? ?? ?? ?? 48 89 6C 24 60 8B EE", 3),
                new("CHARACTER_ADDRESS", "48 8B ?? ?? ?? ?? ?? 80 B8 DC 00 00 00 00 0F 85", 3),
                new("SAVE_ADDRESS", "48 8B 15 ?? ?? ?? ?? 45 33 C0 48 8B CE E8 ?? ?? ?? ?? 84 C0 74 1A 48 8B", 3),
                new("WEAPON_ADDRESS", "48 8B 0D ?? ?? ?? ?? 8B 51 70 39 93", 3),
                new("ABNORMALITIES_ADDRESS", "48 8B 15 ?? ?? ?? ?? 48 85 D2 74 08 48 8B CB E8 ?? ?? ?? ?? 48 8B 15 ?? ?? ?? ?? 48 85 D2 74 08 48 8B CB E8 ?? ?? ?? ?? 48 8B 15 ?? ?? ?? ?? 48 85 D2 74 0D", 3),
                // Missing: ARGOSY_ADDRESS
                new("TRAINING_DOJO_ADDRESS", "48 8B 05 ?? ?? ?? ?? 48 8B E9 48 85 C0 74 0B 48 83 78 10 00 74 04 32 D2", 3),
                new("CHAT_ADDRESS", "48 8B 05 ?? ?? ?? ?? 48 8D 4C 24 ?? 48 8B D7 4C 8B 40 ?? E8 ?? ?? ?? ?? 0F 10 00", 3),
                new("CHAT_UI_ADDRESS", "48 8B 05 ?? ?? ?? ?? 83 B8 ?? ?? ?? ?? 00 74 27", 3),
                new("SESSION_PLAYERS_ADDRESS", "48 8B 15 ?? ?? ?? ?? 45 33 C0 48 8B CE E8 ?? ?? ?? ?? 84 C0 74 1A 44 0F B6", 3),
                new("UI_ADDRESS", "48 8B 05 ?? ?? ?? ?? 83 B8 ?? ?? ?? ?? 00 74 27", 3),
                new("MOUSE_ADDRESS", "48 8B 05 ?? ?? ?? ?? 4C 8B 5B ?? 0F 29 44 24", 3),
                new("QUEST_ADDRESS", "48 8B 05 ?? ?? ?? ?? 83 B8 ?? ?? ?? ?? 02 75 ?? 83 B8 ?? ?? ?? ?? 07", 3),
                new("MEOWMASTERS_ADDRESS", "C7 82 ?? ?? ?? ?? ?? ?? ?? ?? 48 8B 3D ?? ?? ?? ?? 85 C0 79", 13),
                new("COHOOT_ADDRESS", "48 8B 05 ?? ?? ?? ?? 33 ED 48 8B 48 ?? 8B 41", 3),
                // Can use this to find DATA_TRAINING_DOJO_ROUNDS_LEFT, just subtract 8 from this address
                new("DATA_TRAINING_UNK_SUBTRACT_8", "8B ?? ?? ?? ?? ?? 0F AF CB 3B C1 0F", 2),
                
                new("FUNC_WIREBUG_HIDE_AIM_ADDRESS", "49 8B 84 24 ?? ?? ?? ?? 48 83 60 50 FE 49 83 BC 24", 8, false),

                // CRC
                new("CRC_FUNC_1", "48 89 5C 24 08 48 89 6C 24 10 48 89 74 24 18 48 89 7C 24 20 41 56 48 83 EC 20 44 8B 05 ?? ?? ?? ?? 48 8B EA 8B 05 ?? ?? ?? ??", 0, false),
                new("CRC_FUNC_2", "48 89 5C 24 08 48 89 6C 24 10 48 89 74 24 18 57 48 83 EC 20 48 8B 5A 10 48 8B F2 48 8B 52 20 48 8B E9", 0, false),
                new("CRC_FUNC_3", "48 89 5C 24 08 48 89 6C 24 10 48 89 74 24 18 57 48 83 EC 20 4C 8B 05 ?? ?? ?? ?? 33 DB", 0, false),
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
