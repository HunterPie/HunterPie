using System.Runtime.InteropServices;

namespace HunterPie.Core.Game.Rise.Definitions
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MHRStageStructure
    {   
        /**
         * 0 -> None (Main menu)
         * 1 -> Title
         * 2 -> CharMake
         * 3 -> SaveLoad (Char selection)
         * 4 -> Village
         * 5 -> Quest
         * 6 -> LastBoss
         * 7 -> LastBoss_MR
         * 8 -> Arena
         * 9 -> Hyakuryu
         * 10 -> Result
         * 11 -> Demo
         * 12 -> Move (Loading screen)
         * **/
        public int Type; // _GameState
        public int VillageId; // _VillageSpace
        public int Unk1; // _VillageSpaceChecker
        public int Section;  // _QuestEnvSpace
        public int Unk2;  // _ReverbBaseSpace
        public int HuntingId;  // _CurrentMapNo

        public bool IsMainMenu() => Type == 0;
        public bool IsSelectingCharacter() => Type == 3;
        public bool IsVillage() => Type == 4;
        public bool IsHuntingZone() => Type >= 5 && Type <= 11;
        public bool IsLoadingScreen() => Type == 12;

        public bool IsIrrelevantStage()
        {
            return IsMainMenu() || IsSelectingCharacter() || IsLoadingScreen();
        }
    }
}
