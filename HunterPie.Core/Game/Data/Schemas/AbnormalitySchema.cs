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
    public string FlagType;
    public string WithFlag;
    public bool IsBuildup;
    public int MaxBuildup;
    public bool IsInfinite;
    public int MaxTimer;
}
