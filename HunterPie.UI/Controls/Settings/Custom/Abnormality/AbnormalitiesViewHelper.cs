using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.Core.Client.Localization;
using HunterPie.Core.Domain.Enums;
using HunterPie.Core.Game.Data;
using HunterPie.UI.Assets.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using System.Xml;

namespace HunterPie.UI.Controls.Settings.Custom.Abnormality
{
    public class AbnormalitiesViewHelper
    {
        private const string RISE_ABNORMALITIES_FILE = "Game/Rise/Data/AbnormalityData.xml";
        private const string WORLD_ABNORMALITIES_FILE = "Game/World/Data/AbnormalityData.xml";
        private const string ALL_ABNORMALITIES_QUERY = "//Abnormalities/*/Abnormality";

        private const string ICON_SONGS = "ICON_SELFIMPROVEMENT";
        private const string ICON_PALICO = "ICON_ORCHESTRA";
        private const string ICON_CONSUMABLES = "ITEM_DEMONDRUG";
        private const string ICON_DEBUFFS = "ICON_VENOM";
        private const string ICON_SKILLS = "ICON_BUILD";
        private const string ICON_FOODS = "ICON_DANGO";

        private static readonly Dictionary<string, ImageSource> Icons = new Dictionary<string, ImageSource>()
        {
            { "Songs", Resources.Icon(ICON_SONGS) },
            { "Palico", Resources.Icon(ICON_PALICO) },
            { "Consumables", Resources.Icon(ICON_CONSUMABLES) },
            { "Debuffs", Resources.Icon(ICON_DEBUFFS) },
            { "Skills", Resources.Icon(ICON_SKILLS) },
            { "Foods", Resources.Icon(ICON_FOODS) },
        };

        public static AbnormalityCollectionViewModel[] GetViewModelsBy(GameProcess game, AbnormalityWidgetConfig config)
        {
            // TODO: Use AbnormalityData class to load abnormalities and then convert them using the IMapper
            string gameDataFile = game switch
            {
                GameProcess.MonsterHunterRiseSunbreakDemo or
                GameProcess.MonsterHunterRise => RISE_ABNORMALITIES_FILE,

                GameProcess.MonsterHunterWorld => WORLD_ABNORMALITIES_FILE,
                _ => throw new NotImplementedException("unreachable")
            };

            Dictionary<string, AbnormalityCollectionViewModel> collections = new(5);
            XmlDocument document = new();
            document.Load(ClientInfo.GetPathFor(gameDataFile));
            XmlNodeList nodes = document.SelectNodes(ALL_ABNORMALITIES_QUERY);

            foreach (XmlNode node in nodes)
            {
                string category = node.ParentNode.Name;
                string categoryString = $"//Strings/Client/Settings/Setting[@Id='ABNORMALITY_{category.ToUpperInvariant()}_STRING']";
                string name = node.Attributes["Name"]?.Value ?? "ABNORMALITY_UNKNOWN";
                string icon = node.Attributes["Icon"]?.Value ?? "ICON_MISSING";
                string id = node.Attributes["Id"].Value;
                string abnormId = AbnormalityData.BuildAbnormalityId(id, category);

                if (!collections.ContainsKey(category))
                    collections.Add(category, new()
                    {
                        Name = Localization.QueryString(categoryString),
                        Description = Localization.QueryDescription(categoryString),
                        Icon = Icons[category]
                    });

                AbnormalityCollectionViewModel collection = collections[category];

                collection.Abnormalities.Add(new()
                {
                    Name = Localization.QueryString($"//Strings/Abnormalities/Abnormality[@Id='{name}']"),
                    Icon = icon,
                    Id = abnormId,
                    Category = Localization.QueryString(categoryString),
                    IsMatch = true,
                    IsEnabled = config.AllowedAbnormalities.Contains(abnormId)
                });
            }

            return collections.Values.ToArray();
        }

    }
}
