using HunterPie.Core.Game.Data.Interfaces;
using HunterPie.Core.Game.Data.Schemas;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml;

namespace HunterPie.Core.Game.Data;

#nullable enable
[Obsolete("AbnormalityData is deprecated and will be removed in a later release, use AbnormalityService instead.")]
public class AbnormalityData
{
    public const string SongPrefix = "Songs_";
    public const string GearPrefix = "Gears_";
    public const string ConsumablePrefix = "Consumables_";
    public const string DebuffPrefix = "Debuffs_";
    public const string SkillPrefix = "Skills_";
    public const string FoodPrefix = "Foods_";
    public const string OrchestraPrefix = "Palico_";
    public const float TIMER_MULTIPLIER = 60.0f;

    public const string Songs = "Songs";
    public const string Gears = "Gears";
    public const string Consumables = "Consumables";
    public const string Debuffs = "Debuffs";
    public const string Skills = "Skills";
    public const string Foods = "Foods";
    public const string Orchestra = "Palico";

    private static XmlDocument _abnormalityData;
    private static IAbnormalityFlagTypeParser? _flagTypeParser;
    public static Dictionary<string, AbnormalitySchema> Abnormalities { get; private set; }


    internal static void Init(string path, IAbnormalityFlagTypeParser? flagTypeParser = null)
    {
        _flagTypeParser = flagTypeParser;
        _abnormalityData = new();
        _abnormalityData.Load(path);

        CreateFixedSizeDictionary();
        LoadAbnormalities();
    }

    private static void CreateFixedSizeDictionary()
    {
        int abnormalitiesCount = _abnormalityData.SelectNodes("//Abnormalities/*/Abnormality").Count;

        Abnormalities = new(abnormalitiesCount);
    }

    private static void LoadAbnormalities()
    {
        XmlNodeList abnormalities = _abnormalityData.SelectNodes("//Abnormalities/*/Abnormality");

        foreach (XmlNode abnormality in abnormalities)
        {
            string id = abnormality.Attributes["Id"].Value;
            string name = abnormality.Attributes["Name"]?.Value ?? "ABNORMALITY_UNKNOWN";
            string icon = abnormality.Attributes["Icon"]?.Value ?? "ICON_MISSING";
            string offset = abnormality.Attributes["Offset"]?.Value ?? id;
            string dependsOn = abnormality.Attributes["DependsOn"]?.Value ?? "0";
            string withValue = abnormality.Attributes["WithValue"]?.Value ?? "0";
            string group = abnormality.ParentNode!.Name;
            string category = abnormality.Attributes["Category"]?.Value ?? group;
            string isBuildup = abnormality.Attributes["IsBuildup"]?.Value ?? "False";
            string maxBuildup = abnormality.Attributes["MaxBuildup"]?.Value ?? "0";
            string isInfinite = abnormality.Attributes["IsInfinite"]?.Value ?? "False";
            string maxTimer = abnormality.Attributes["MaxTimer"]?.Value ?? "0";
            string flagType = abnormality.Attributes["FlagType"]?.Value ?? "None";
            string flag = abnormality.Attributes["Flag"]?.Value ?? "None";

            AbnormalitySchema schema = new()
            {
                Id = BuildAbnormalityId(id, group),
                Name = name,
                Icon = icon,
                Category = category,
                Group = group,
            };

            int.TryParse(offset, NumberStyles.HexNumber, null, out schema.Offset);
            int.TryParse(dependsOn, NumberStyles.HexNumber, null, out schema.DependsOn);
            int.TryParse(withValue, out schema.WithValue);
            bool.TryParse(isBuildup, out schema.IsBuildup);
            int.TryParse(maxBuildup, out schema.MaxBuildup);
            bool.TryParse(isInfinite, out schema.IsInfinite);
            int.TryParse(maxTimer, out schema.MaxTimer);
            Enum.TryParse(flagType, out schema.FlagType);

            schema.Flag = _flagTypeParser?.Parse(schema.FlagType, flag);

            Abnormalities.Add(schema.Id, schema);
        }
    }

    public static string BuildAbnormalityId(string self, string group)
    {
        return self.StartsWith("ABN_")
            ? self
            : $"{group}_{self}";
    }

    public static AbnormalitySchema[] GetAllAbnormalitiesFromCategory(string category)
    {
        AbnormalitySchema[] abnormalities = Abnormalities.Where(e => e.Value.Category == category)
                                                            .Select(el => el.Value)
                                                            .ToArray();

        return abnormalities;
    }
}
