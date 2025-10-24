using HunterPie.Core.Extensions;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Markup;

namespace HunterPie.UI.Architecture.Markup;

[MarkupExtensionReturnType(typeof(ObservableCollection<int>))]
public class CreateCollection : MarkupExtension
{
    public int ItemsCount { get; set; } = 0;

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return Enumerable.Range(0, ItemsCount)
            .ToObservableCollection();
    }
}