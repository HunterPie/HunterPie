using HunterPie.Core.Game.Enums;
using System;

namespace HunterPie.Core.Game.Data.Schemas;

#nullable enable
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
    public AbnormalityFlagType FlagType;
    public Enum? Flag;
    public bool IsBuildup;
    public int MaxBuildup;
    public bool IsInfinite;
    public int MaxTimer;

    public T? GetFlagAs<T>()
    {
        if (Flag is T flag)
            return flag;

        return default(T);
    }
}
