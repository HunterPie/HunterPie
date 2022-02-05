using HunterPie.Core.Game.Data.Schemas;
using HunterPie.Core.Logger;
using System.Collections.Generic;
using System.Xml;

namespace HunterPie.Core.Game.Data
{
    public class AbnormalityData
    {
        public const string SongPrefix = "Songs_";

        private static XmlDocument _abnormalityData;
        public static Dictionary<string, AbnormalitySchema> Abnormalities { get; private set; }

        internal static void Init(string path)
        {
            _abnormalityData = new();
            _abnormalityData.Load(path);

            CreateFixedSizeDictionary();
            LoadSongAbnormalities();
        }

        private static void CreateFixedSizeDictionary()
        {
            int abnormalitiesCount = _abnormalityData.SelectNodes("/Abnormality").Count;

            Abnormalities = new(abnormalitiesCount);
        }

        private static void LoadSongAbnormalities()
        {
            XmlNodeList abnormalities = _abnormalityData.SelectNodes("//Abnormalities/Songs/Abnormality");

            foreach (XmlNode abnormality in abnormalities)
            {
                string id = abnormality.Attributes["Id"].Value;
                string name = abnormality.Attributes["Name"]?.Value ?? "ABNORMALITY_UNKNOWN";
                string icon = abnormality.Attributes["Icon"]?.Value ?? "ICON_MISSING";
                string finalId = $"{SongPrefix}{id}";

                AbnormalitySchema schema = new()
                {
                    Id = finalId,
                    Name = name,
                    Icon = icon
                };

                Abnormalities.Add(schema.Id, schema);
            }
        }

        public static AbnormalitySchema? GetSongAbnormalityData(int id)
        {
            if (!Abnormalities.ContainsKey($"{SongPrefix}{id}"))
                return null;

            AbnormalitySchema schema = Abnormalities[$"{SongPrefix}{id}"];

            if (schema.Icon == "ICON_MISSING")
                Log.Debug($"Missing abnormality {SongPrefix}{id}");

            return schema;
        }
    }
}
