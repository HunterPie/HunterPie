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
            return new ImageSourceConverter().ConvertFromString($"pack://siteoforigin:,,,/Assets/Monsters/Icons/{MonsterEm}_ID.png");
        }
    }
}
