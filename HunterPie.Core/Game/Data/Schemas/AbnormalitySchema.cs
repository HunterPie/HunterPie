using HunterPie.Core.Game.Enums;

namespace HunterPie.Core.Game.Data.Schemas;

public struct AbnormalitySchema
{
    public string Id;
    public int Offset;
    public int SubPtr;
    public int DependsOn;
    public int WithValue;
    public string Name;
    public string Icon;
    public string Category;
    public string Group;
    public AbnormalityFlagType FlagType;
    public string Flag;
    public bool IsBuildup;
    public int MaxBuildup;
    public bool IsInfinite;
    public int MaxTimer;
}
