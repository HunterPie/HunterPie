using HunterPie.Core.Client.Localization;
using System;
using System.ComponentModel;
using System.Windows.Markup;

namespace HunterPie.UI.Architecture.Assets;

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