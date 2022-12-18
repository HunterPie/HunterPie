namespace HunterPie.Core.Game.Data.Schemas;
using HunterPie.Core.Game.Enums;

public struct AbnormalitySchema
{
    public string Id;
    public int Offset;
    public int DependsOn;
    public AbnormalityCompareType CompareOperator;
    public int WithValue;
    public int WithValueNot;
    public string Name;
    public string Icon;
    public string Category;
    public string Group;
    public bool IsBuildup;
    public int MaxBuildup;
    public bool IsInfinite;
}
