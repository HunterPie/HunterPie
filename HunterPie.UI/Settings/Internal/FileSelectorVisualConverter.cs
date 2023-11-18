﻿using HunterPie.Core.Domain.Enums;
using HunterPie.Core.Settings.Types;
using HunterPie.UI.Settings.Converter;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace HunterPie.UI.Settings.Internal;

internal class FileSelectorVisualConverter : IVisualConverter
{
    public FrameworkElement Build(GameProcess? game, object parent, PropertyInfo childInfo)
    {
        var child = (IFileSelector)childInfo.GetValue(parent);

        Binding binding = VisualConverterHelper.CreateBinding(child, nameof(IFileSelector.Current));

        ComboBox ui = new()
        {
            ItemsSource = child.Elements,
            MinHeight = 35,
            VerticalAlignment = VerticalAlignment.Center
        };

        _ = BindingOperations.SetBinding(ui, ComboBox.SelectedValueProperty, binding);
        return ui;
    }
}
