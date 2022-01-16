using HunterPie.Core.Client;
using HunterPie.Core.Client.Localization;
using HunterPie.Core.Domain.Process;
using HunterPie.Core.Logger;
using System.IO;
using System.Xml;

namespace HunterPie.Core.Game.Rise.Data
{
    public class MHRStrings
    {

        private readonly IProcessManager _process;

        public MHRStrings(IProcessManager process)
        {
            _process = process;

        }

        public string GetMonsterNameById(int id)
        {
            string query = string.Format("//Strings/Monsters/Rise/Monster[@Id='%d']", id);
            XmlNode monster = Localization.Query(query);

            return monster?.Attributes["String"].Value ?? "Unknown";
        }

        public string GetStageNameById(int id)
        {
            string query = string.Format("//Strings/Stages/Rise/Stage[@Id='%d']", id);
            XmlNode monster = Localization.Query(query);

            return monster?.Attributes["String"].Value ?? "Unknown";
        }
    }
}
