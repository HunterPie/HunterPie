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
            bool isRise = MonsterEm.StartsWith("Rise");
            string imageId = MonsterEm;

            if (!isRise)
                imageId += "_ID";

            return new ImageSourceConverter()
                .ConvertFromString($"pack://siteoforigin:,,,/Assets/Monsters/Icons/{imageId}.png");
        }
    }
}
