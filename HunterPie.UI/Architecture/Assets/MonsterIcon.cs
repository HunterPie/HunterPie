using System;
using System.Windows.Markup;
using System.Windows.Media;

namespace HunterPie.UI.Architecture.Assets;

[MarkupExtensionReturnType(typeof(ImageSource))]
public class MonsterIcon(string monsterEm) : MarkupExtension
{
    public string MonsterEm { get; set; } = monsterEm;

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return $"pack://siteoforigin:,,,/Assets/Monsters/Icons/{MonsterEm}.png";
    }
}