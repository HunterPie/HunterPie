using HunterPie.Core.Client.Localization;
using HunterPie.Core.Client.Localization.Entity;
using HunterPie.DI;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Markup;

namespace HunterPie.UI.Architecture.Markup;

[MarkupExtensionReturnType(typeof(string))]
public class Localization(string id) : MarkupExtension
{
    private static readonly DependencyObject Dummy = new();

    private static ILocalizationRepository Repository => DependencyContainer.Get<ILocalizationRepository>();

    private readonly string _id = id;

    public bool Description { get; init; } = false;

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        if (DesignerProperties.GetIsInDesignMode(Dummy))
            return "String";

        LocalizationData localization = Repository.FindBy(_id);

        return Description ? localization.Description : localization.String;
    }
}