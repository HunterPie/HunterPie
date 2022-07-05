using System.Runtime.InteropServices;

namespace HunterPie.Core.Game.Rise.Definitions
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MHRStageStructure
    {   
        /**
         * 0 -> Main menu
         * 4 -> Village
         * 5 -> Hunting Zones
         * 12 -> Loading screen
         * **/
        public int Type;
        public int VillageId; // Probably map section?
        public int Unk1;
        public int Section;
        public int Unk2;
        public int HuntingId;

        public bool IsMainMenu() => Type == 0;
        public bool IsVillage() => Type == 4;
        public bool IsHuntingZone() => Type == 5;
        public bool IsLoadingScreen() => Type == 12;
    }
}
