using HunterPie.Core.Settings.Types;
using HunterPie.UI.Settings.Converter;
using System.Reflection;
using System.Windows;
using System.Windows.Data;
using RangeUI = HunterPie.UI.Controls.Sliders.Range;

namespace HunterPie.UI.Settings.Internal;

internal class RangeVisualConverter : IVisualConverter
{
    public FrameworkElement Build(object parent, PropertyInfo childInfo)
    {
        var range = (Range)childInfo.GetValue(parent);
        Binding currentBinding = VisualConverterHelper.CreateBinding(range, nameof(Range.Current));
        Binding maxBinding = VisualConverterHelper.CreateBinding(range, nameof(Range.Max));
        Binding minBinding = VisualConverterHelper.CreateBinding(range, nameof(Range.Min));
        Binding stepBinding = VisualConverterHelper.CreateBinding(range, nameof(Range.Step));

        RangeUI slider = new();

        _ = BindingOperations.SetBinding(slider, RangeUI.ValueProperty, currentBinding);
        _ = BindingOperations.SetBinding(slider, RangeUI.MaximumProperty, maxBinding);
        _ = BindingOperations.SetBinding(slider, RangeUI.MinimumProperty, minBinding);
        _ = BindingOperations.SetBinding(slider, RangeUI.ChangeProperty, stepBinding);

        return slider;
    }
}
