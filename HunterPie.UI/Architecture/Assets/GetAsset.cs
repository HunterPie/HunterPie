using HunterPie.Core.Client;
using HunterPie.Core.Client.Localization;
using HunterPie.Core.Remote;
using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Markup;
using System.Windows.Media;

namespace HunterPie.UI.Architecture.Assets;

[MarkupExtensionReturnType(typeof(ImageSource))]
public class MonsterIcon(string monsterEm) : MarkupExtension
{
    public string MonsterEm { get; set; } = monsterEm;

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        static async void DownloadIcon(string monsterEm) => await CDN.GetMonsterIconUrl(monsterEm);
        string imagePath = ClientInfo.GetPathFor($"Assets/Monsters/Icons/{MonsterEm}.png");

        // If file doesn't exist locally, we can check for the CDN
        if (!File.Exists(imagePath))
            DownloadIcon(MonsterEm);

        return $"pack://siteoforigin:,,,/Assets/Monsters/Icons/{MonsterEm}.png";
    }
}

[MarkupExtensionReturnType(typeof(string))]
[Obsolete("Use Localization instead")]
public class LocalizationString(string localizationId) : MarkupExtension
{
    public string LocalizationId { get; set; } = localizationId;

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        if (DesignerProperties.GetIsInDesignMode(new()))
            return "String";

        return Localization.QueryString(LocalizationId);
    }

}

[MarkupExtensionReturnType(typeof(string))]
[Obsolete("Use Localization instead")]
public class LocalizationDescription(string localizationId) : MarkupExtension
{
    public string LocalizationId { get; set; } = localizationId;

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        if (DesignerProperties.GetIsInDesignMode(new()))
            return "Description";

        return Localization.QueryDescription(LocalizationId);
    }
}