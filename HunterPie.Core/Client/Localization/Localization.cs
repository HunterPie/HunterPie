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
            string englishXml = Path.Combine(ClientInfo.LanguagesPath, "en-us.xml");
            document = new();
            document.Load(englishXml);

            string xmlPath = Path.Combine(ClientInfo.LanguagesPath, ClientConfig.Config.Client.Language);

            if (!File.Exists(xmlPath))
            {
                Log.Error($"Failed to find {Path.GetFileNameWithoutExtension(xmlPath)} localization");
                return;
            }
            
            try
            {
                if (ClientConfig.Config.Client.Language != "en-us.xml")
                {
                    XmlDocument otherLanguage = new();
                    otherLanguage.Load(xmlPath);

                    XmlNodeList englishNodes = document.DocumentElement.SelectNodes("//*");
                    foreach (XmlNode node in otherLanguage.DocumentElement.SelectNodes("//*"))
                    {
                        string id = node.Attributes["Id"]?.Value;

                        if (id is null)
                            continue;

                        XmlNode match = document.DocumentElement.SelectSingleNode($"//{node.ParentNode.Name}/*[@Id='{id}']");

                        if (match is null)
                            continue;

                        match.Attributes["String"].Value = node.Attributes["String"].Value;
                        match.Attributes["Description"].Value = node.Attributes["Description"].Value;
                    }
                }
            } catch(Exception err) { Log.Error(err); }
            

            Log.Info($"Loaded localization {Path.GetFileNameWithoutExtension(xmlPath)}");
        }

        public static XmlNode Query(string query) => _instance.document.SelectSingleNode(query);
        public static XmlNodeList QueryMany(string query) => _instance.document.SelectNodes(query);
    }
}
