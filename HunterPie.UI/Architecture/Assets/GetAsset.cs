using HunterPie.Core.Client;
using HunterPie.Core.Client.Localization;
using HunterPie.Core.Remote;
using HunterPie.Core.Utils;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Markup;
using System.Windows.Media;

namespace HunterPie.UI.Architecture.Assets
{
    [MarkupExtensionReturnType(typeof(ImageSource))]
    public class MonsterIcon : MarkupExtension
    {
        public string MonsterEm { get; set; }

        public MonsterIcon(string monsterEm)
        {
            MonsterEm = monsterEm;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            async Task<string> DownloadMonsterIcon()
            {
                return await CDN.GetMonsterIconUrl(MonsterEm);
            }
            string imagePath = ClientInfo.GetPathFor($"Assets/Monsters/Icons/{MonsterEm}.png");

            // If file doesn't exist locally, we can check for the CDN
            if (!File.Exists(imagePath))
                AsyncHelper.RunSync(DownloadMonsterIcon);

            return $"pack://siteoforigin:,,,/Assets/Monsters/Icons/{MonsterEm}.png";
        }
    }

    [MarkupExtensionReturnType(typeof(string))]
    public class LocalizationString : MarkupExtension
    {
        public string LocalizationId { get; set; }

        public LocalizationString(string localizationId)
        {
            LocalizationId = localizationId;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Localization.Query(LocalizationId)?.Attributes["String"].Value;
        }
    }
}
