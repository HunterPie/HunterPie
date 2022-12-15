namespace HunterPie.Core.Game.Data.Schemas;

public enum CompareType
{
    WithValue,
    WithValueNot
}

public struct AbnormalitySchema
{
    public string Id;
    public int Offset;
    public int DependsOn;
    public int CompareOpr;
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
