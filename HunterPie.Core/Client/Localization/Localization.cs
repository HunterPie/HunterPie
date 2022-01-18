using HunterPie.Core.Logger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace HunterPie.Core.Client.Localization
{
    public class Localization
    {

        private XmlDocument document;
        private static Localization _instance;

        public static Localization Instance
        {
            get
            {
                if (_instance is null)
                    _instance = new();

                return _instance;
            }
        }

        private Localization()
        {
            document = new();

            string xmlPath = Path.Combine(ClientInfo.LanguagesPath, ClientConfig.Config.Client.Language);

            if (!File.Exists(xmlPath))
                xmlPath = Path.Combine(ClientInfo.LanguagesPath, "en-us.xml");

            document.Load(xmlPath);

            Log.Info($"Loaded localization {Path.GetFileNameWithoutExtension(xmlPath)}");
        }

        public static XmlNode Query(string query) => _instance.document.SelectSingleNode(query);
        public static XmlNodeList QueryMany(string query) => _instance.document.SelectNodes(query);
    }
}
