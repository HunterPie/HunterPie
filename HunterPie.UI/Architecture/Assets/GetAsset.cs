using HunterPie.Core.Client;
using HunterPie.Core.Client.Localization;
using HunterPie.Core.Remote;
using System;
using System.IO;
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
            string imageId = MonsterEm;

            string imagePath = ClientInfo.GetPathFor($"Assets/Monsters/Icons/{imageId}.png");

            // If file doesn't exist locally, we can check for the CDN
            if (!File.Exists(imagePath))
                CDN.GetMonsterIconUrl(imageId).RunSynchronously();

            return new ImageSourceConverter()
                .ConvertFromString($"pack://siteoforigin:,,,/Assets/Monsters/Icons/{imageId}.png");
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
