using HunterPie.Core.Game.Data.Schemas;
using System.Collections.Generic;
using System.Xml;

namespace HunterPie.Core.Game.Data
{
    public class MonsterData
    {
        private static XmlDocument _monsterXmlDocument;
        public static Dictionary<int, MonsterDataSchema> Monsters { get; private set; }
        
        internal static void Init(string path)
        {
            _monsterXmlDocument = new();

            _monsterXmlDocument.Load(path);
            ParseAllMonsters();
        }

        private static void ParseAllMonsters()
        {
            XmlNodeList monsters = _monsterXmlDocument.SelectNodes("//GameData/Monsters/Monster");

            Monsters = new Dictionary<int, MonsterDataSchema>(monsters.Count);
            int i = 0;
            foreach (XmlNode monster in monsters)
            {
                XmlNodeList parts = monster.SelectNodes("Parts/Part");
                MonsterPartSchema[] partsArray = new MonsterPartSchema[parts.Count];

                for (int j = 0; j < partsArray.Length; j++)
                {
                    partsArray[j] = new()
                    {
                        Id = int.Parse(parts[j].Attributes["Id"].Value),
                        String = parts[j].Attributes["String"].Value
                    };
                }

                MonsterDataSchema schema = new()
                {
                    Id = int.Parse(monster.Attributes["Id"].Value),
                    Parts = partsArray
                };

                Monsters.Add(schema.Id, schema);

                i++;
            }
        }

        public static MonsterDataSchema GetMonsterData(int id)
        {
            if (!Monsters.ContainsKey(id))
                return null;

            return Monsters[id];
        }

        public static MonsterPartSchema GetMonsterPartData(int id, int index)
        {
            if (!Monsters.ContainsKey(id))
                return null;

            if (Monsters[id].Parts.Length == 0 || index >= Monsters[id].Parts.Length)
                return null;

            return Monsters[id].Parts[index];
        }
    }
}
