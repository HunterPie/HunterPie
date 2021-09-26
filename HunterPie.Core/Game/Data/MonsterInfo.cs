namespace HunterPie.Core.Game.Data
{
    public struct CrownInfo
    {
        public float Mini;
        public float Silver;
        public float Gold;
    }

    public struct WeaknessInfo
    {
        public string Id;
        public int Stars;
    }

    public struct PartInfo
    {
        public string Id;
        public bool IsRemovable;
        public string GroupId;
        public ThresholdInfo[] BreakThresholds;
        public bool Skip;
        public uint Index;
        public uint[] TenderizeIds;
    }

    public struct ThresholdInfo
    {
        public int Threshold;
        public bool HasConditions;
        public float MinHealth;
        public int MinFlinch;
    }

    public struct MonsterInfo
    {
        public string Em;
        public int Id;
        public CrownInfo Crowns;
        public float Capture;
        public WeaknessInfo[] Weaknesses;

        public int MaxParts;
        public PartInfo[] Parts;
        public int MaxRemovableParts;
    }
}
