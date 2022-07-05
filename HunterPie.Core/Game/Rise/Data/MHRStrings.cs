using HunterPie.Core.Client.Localization;
using HunterPie.Core.Domain.Process;
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
            string query = $"//Strings/Monsters/Rise/Monster[@Id='{id}']";
            XmlNode monster = Localization.Query(query);

            return monster?.Attributes["String"]?.Value ?? $"Unknown [id: {id}]";
        }

        public string GetStageNameById(int id)
        {
            string query = $"//Strings/Stages/Rise/Stage[@Id='{id}']";
            XmlNode monster = Localization.Query(query);

            return monster?.Attributes["String"]?.Value ?? $"Unknown [id: {id}]";
        }
    }
}
