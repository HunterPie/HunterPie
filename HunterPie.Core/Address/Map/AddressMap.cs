using HunterPie.Core.Address.Map.Internal;
using System.IO;

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

        public static T Get<T>(string key) => parser.Get<T>(key);
        public static void Add<T>(string key, T value) => parser.Add<T>(key, value);

    }
}
