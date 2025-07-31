using HunterPie.Core.Client.Localization;
using HunterPie.DI;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Markup;

namespace HunterPie.UI.Architecture.Markup;

[MarkupExtensionReturnType(typeof(string))]
public class Localization : MarkupExtension
{
    private static readonly DependencyObject Dummy = new();

    private ILocalizationRepository Repository => DependencyContainer.Get<ILocalizationRepository>();

    private readonly string _id;

    public Localization(string id)
    {
        _id = id;
    }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        if (DesignerProperties.GetIsInDesignMode(Dummy))
            return "String";

        return Repository.FindStringBy(_id);
    }
}