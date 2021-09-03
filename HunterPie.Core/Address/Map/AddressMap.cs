using HunterPie.Core.Address.Map.Internal;
using System.IO;
using System.Linq;

namespace HunterPie.Core.Address.Map
{
    public static class AddressMap
    {

        private static IAddressMapParser parser;

        public static void Parse(string filePath)
        {
            using (FileStream file = File.OpenRead(filePath))
            {
                StreamReader stream = new StreamReader(file);
                
                parser = new LegacyAddressMapParser(stream);
            }
        }

        public static void ParseLatest(string mapsDir)
        {
            string latestMap = Directory.GetFiles(mapsDir, "*.map")
                .OrderByDescending(version => version)
                .First();
              
            Parse(Path.Combine(mapsDir, latestMap));
        }

        public static T Get<T>(string key) => parser.Get<T>(key);
        public static long GetAbsolute(string key) => parser.Get<long>("BASE") + parser.Get<long>(key);
        public static void Add<T>(string key, T value) => parser.Add(key, value);

    }
}
