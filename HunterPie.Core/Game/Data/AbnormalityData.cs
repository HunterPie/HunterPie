using HunterPie.Core.Game.Data.Schemas;
using HunterPie.Core.Logger;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml;

namespace HunterPie.Core.Game.Data
{
    public class AbnormalityData
    {
        public const string SongPrefix = "Songs_";
        public const string ConsumablePrefix = "Consumables_";

        private static XmlDocument _abnormalityData;
        public static Dictionary<string, AbnormalitySchema> Abnormalities { get; private set; }

        internal static void Init(string path)
        {
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
                string offset = abnormality.Attributes["Offset"]?.Value ?? "0";
                string finalId = $"{abnormality.ParentNode.Name}_{id}";

                AbnormalitySchema schema = new()
                {
                    Id = finalId,
                    Name = name,
                    Icon = icon
                };

                int.TryParse(offset, NumberStyles.HexNumber, null, out schema.Offset);

                Abnormalities.Add(schema.Id, schema);
            }
        }

        public static AbnormalitySchema? GetSongAbnormalityData(int id)
        {
            if (!Abnormalities.ContainsKey($"{SongPrefix}{id}"))
                return null;

            AbnormalitySchema schema = Abnormalities[$"{SongPrefix}{id}"];

            if (schema.Icon == "ICON_MISSING")
                Log.Info($"Missing abnormality {SongPrefix}{id}");

            return schema;
        }

        public static AbnormalitySchema? GetConsumableAbnormalityData(int id)
        {
            if (!Abnormalities.ContainsKey($"{ConsumablePrefix}{id:X}"))
                return null;

            AbnormalitySchema schema = Abnormalities[$"{ConsumablePrefix}{id:X}"];

            if (schema.Icon == "ICON_MISSING")
                Log.Info($"Missing abnormality {ConsumablePrefix}{id:X}");

            return schema;
        }

        public static AbnormalitySchema[] GetAllConsumableAbnormalities()
        {
            AbnormalitySchema[] abnormalities = Abnormalities.Where(e => e.Key.StartsWith(ConsumablePrefix))
                                                                .Select(el => el.Value)
                                                                .ToArray();

            return abnormalities;
        }
    }
}
