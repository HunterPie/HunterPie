using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration.Enums;
using HunterPie.Core.Domain.Mapper;
using HunterPie.Core.Game.Data.Schemas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace HunterPie.Core.Game.Services;

#nullable enable
public static class AbnormalityService
{
    private const string RISE_ABNORMALITIES_FILE = "Game/Rise/Data/AbnormalityData.xml";
    private const string WORLD_ABNORMALITIES_FILE = "Game/World/Data/AbnormalityData.xml";

    private static readonly Lazy<Dictionary<string, AbnormalitySchema>> LazyRiseDataSource = new(() => Load(RISE_ABNORMALITIES_FILE));
    private static readonly Lazy<Dictionary<string, AbnormalitySchema>> LazyWorldDataSource = new(() => Load(WORLD_ABNORMALITIES_FILE));

    /// <summary>
    /// Returns an abnormality with the given internal unique identifier
    /// </summary>
    /// <param name="game">The game type</param>
    /// <param name="id">The abnormality id</param>
    public static AbnormalitySchema? FindBy(GameType game, string id)
    {
        Dictionary<string, AbnormalitySchema> dataSource = GetDataSourceBy(game);

        return dataSource.ContainsKey(id) ? dataSource[id] : null;
    }

    /// <summary>
    /// Returns an array with all abnormalities that match the given category
    /// </summary>
    /// <param name="game">The game type</param>
    /// <param name="category">The category name</param>
    /// <returns>An array with the abnormalities</returns>
    public static AbnormalitySchema[] FindAllAbnormalitiesBy(GameType game, string category)
    {
        Dictionary<string, AbnormalitySchema> dataSource = GetDataSourceBy(game);

        return dataSource.Values.Where(it => it.Category == category)
            .ToArray();
    }

    private static Dictionary<string, AbnormalitySchema> GetDataSourceBy(GameType game)
    {
        Lazy<Dictionary<string, AbnormalitySchema>> dataSource = game switch
        {
            GameType.Rise => LazyRiseDataSource,
            GameType.World => LazyWorldDataSource,
            _ => throw new ArgumentOutOfRangeException(nameof(game), game, null)
        };

        return dataSource.Value;
    }

    private static Dictionary<string, AbnormalitySchema> Load(string file)
    {
        XmlDocument document = new();
        document.Load(ClientInfo.GetPathFor(file));

        return LoadAbnormalities(document);
    }

    private static Dictionary<string, AbnormalitySchema> LoadAbnormalities(XmlDocument document)
    {
        XmlNodeList? abnormalityNodes = document.SelectNodes("//Abnormalities/*/Abnormality");

        if (abnormalityNodes is not { })
            return new Dictionary<string, AbnormalitySchema>();

        int abnormalityCount = abnormalityNodes.Count;

        var dictionary = new Dictionary<string, AbnormalitySchema>(abnormalityCount);

        foreach (XmlNode node in abnormalityNodes)
        {
            AbnormalitySchema schema = MapFactory.Map<XmlNode, AbnormalitySchema>(node);
            dictionary[schema.Id] = schema;
        }

        return dictionary;
    }
}
