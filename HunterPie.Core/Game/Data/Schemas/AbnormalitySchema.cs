namespace HunterPie.Core.Game.Data.Schemas;
using HunterPie.Core.Game.Enums;

public struct AbnormalitySchema
{
    public string Id;
    public int Offset;
    public int DependsOn;
    public int WithValue;
    public string Name;
    public string Icon;
    public string Category;
    public string Group;
    public bool IsBuildup;
    public int MaxBuildup;
    public bool IsInfinite;
    public int MaxTimer;
}
