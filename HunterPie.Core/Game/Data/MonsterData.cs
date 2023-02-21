﻿using HunterPie.Core.Game.Data.Schemas;
using HunterPie.Core.Game.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml;

#nullable enable
namespace HunterPie.Core.Game.Data;

public class MonsterData
{
    private static XmlDocument _monsterXmlDocument;
    public static Dictionary<int, MonsterDataSchema> Monsters { get; private set; }
    public static Dictionary<int, AilmentDataSchema> Ailments { get; private set; }

    internal static void Init(string path)
    {
        _monsterXmlDocument = new();

        _monsterXmlDocument.Load(path);
        ParseAllMonsters();
        ParseAllAilments();
    }

    private static void ParseAllAilments()
    {
        XmlNodeList ailments = _monsterXmlDocument.SelectNodes("//GameData/Ailments/Ailment");

        Ailments = new(ailments.Count);
        foreach (XmlNode ailment in ailments)
        {
            int id = int.Parse(ailment.Attributes["Id"].Value);
            AilmentDataSchema ailm = new()
            {
                String = ailment.Attributes["String"]?.Value
            };

            Ailments[id] = ailm;
        }
    }

    private static void ParseAllMonsters()
    {
        XmlNodeList monsters = _monsterXmlDocument.SelectNodes("//GameData/Monsters/Monster");

        Monsters = new Dictionary<int, MonsterDataSchema>(monsters.Count);
        int i = 0;
        foreach (XmlNode monster in monsters)
        {
            XmlNodeList parts = monster.SelectNodes("Parts/Part");
            var partsArray = new MonsterPartSchema[parts.Count];

            for (int j = 0; j < partsArray.Length; j++)
            {
                partsArray[j] = new()
                {
                    Id = int.Parse(parts[j].Attributes["Id"].Value),
                    String = parts[j].Attributes["String"].Value,
                    TenderizeIds = ParseTenderizeIds(parts[j].Attributes["TenderizeIds"]?.Value)
                };

                _ = bool.TryParse(parts[j].Attributes["IsSeverable"]?.Value, out partsArray[j].IsSeverable);
            }

            MonsterSizeSchema sizeSchema = ParseSize(monster);
            Element[] weaknesses = ParseWeakness(monster);

            MonsterDataSchema schema = new()
            {
                Id = int.Parse(monster.Attributes["Id"].Value),
                Capture = int.Parse(monster.Attributes["Capture"]?.Value ?? "0"),
                IsNotCapturable = bool.Parse(monster.Attributes["IsNotCapturable"]?.Value ?? "false"),
                Size = sizeSchema,
                Parts = partsArray,
                Weaknesses = weaknesses
            };

            Monsters.Add(schema.Id, schema);

            i++;
        }
    }

    private static uint[] ParseTenderizeIds(string? tenderizeIds)
    {
        return tenderizeIds is not null
            ? tenderizeIds.Split(',')
                               .Select(id => uint.Parse(id))
                               .ToArray()
            : Array.Empty<uint>();
    }

    private static MonsterSizeSchema ParseSize(XmlNode monster)
    {
        XmlNode crowns = monster.SelectSingleNode("Crowns")!;

        float mini = 0.9f;
        float silver = 1.15f;
        float gold = 1.23f;

        _ = float.TryParse(
            monster.Attributes!["Size"]?.Value,
            NumberStyles.Float,
            CultureInfo.InvariantCulture,
            out float size
        );

        if (crowns is not null)
        {

            if (crowns.Attributes!["Mini"]?.Value is string miniValue)
            {
                _ = float.TryParse(
                    miniValue,
                    NumberStyles.Float,
                    CultureInfo.InvariantCulture,
                    out mini
                );
            }

            if (crowns.Attributes["Silver"]?.Value is string silverValue)
            {
                _ = float.TryParse(
                    silverValue,
                    NumberStyles.Float,
                    CultureInfo.InvariantCulture,
                    out silver
                );
            }

            if (crowns.Attributes["Gold"]?.Value is string goldValue)
            {
                _ = float.TryParse(
                    goldValue,
                    NumberStyles.Float,
                    CultureInfo.InvariantCulture,
                    out gold
                );
            }
        }

        return new()
        {
            Size = size,
            Mini = mini,
            Silver = silver,
            Gold = gold
        };
    }

    private static Element[] ParseWeakness(XmlNode monster)
    {
        XmlNodeList weaknesses = monster.SelectNodes("Weaknesses/Weakness")!;
        var elements = new Element[weaknesses.Count];

        for (int i = 0; i < weaknesses.Count; i++)
        {
            XmlNode weakness = weaknesses[i]!;

            _ = Enum.TryParse(typeof(Element), weakness!.Attributes!["Name"]?.Value, out object? element);

            if (element is null)
                continue;

            elements[i] = (Element)element;
        }

        return elements;
    }

    public static MonsterDataSchema? GetMonsterData(int id) => !Monsters.ContainsKey(id) ? null : Monsters[id];

    public static MonsterPartSchema? GetMonsterPartData(int id, int index)
    {
        return !Monsters.ContainsKey(id)
            ? null
            : Monsters[id].Parts.Length == 0 || index >= Monsters[id].Parts.Length ? null : Monsters[id].Parts[index];
    }

    public static AilmentDataSchema GetAilmentData(int id) => !Ailments.ContainsKey(id) ? new AilmentDataSchema() { String = $"{id}_UNKNOWN", IsUnknown = true } : Ailments[id];
}
#nullable restore
