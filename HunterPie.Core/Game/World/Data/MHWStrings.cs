using HunterPie.Core.Client.Localization;
using HunterPie.Core.Domain.Process;
using System.Xml;

namespace HunterPie.Core.Game.World.Data
{
    public class MHWStrings
    {
        private readonly IProcessManager _process;

        public MHWStrings(IProcessManager process)
        {
            _process = process;
        }

        public string GetMonsterNameById(int id)
        {
            string query = $"//Strings/Monsters/World/Monster[@Id='{id}']";
            XmlNode monster = Localization.Query(query);

            return monster?.Attributes["String"].Value ?? "Unknown";
        }

        public string GetStageNameById(int id)
        {
            string query = $"//Strings/Stages/World/Stage[@Id='{id}']";
            XmlNode monster = Localization.Query(query);

            return monster?.Attributes["String"].Value ?? "Unknown";
        }
    }
}
