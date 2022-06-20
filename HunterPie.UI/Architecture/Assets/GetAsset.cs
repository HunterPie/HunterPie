using HunterPie.Core.Client.Localization;
using System;
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
