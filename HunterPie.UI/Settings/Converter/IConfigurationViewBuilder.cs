using HunterPie.UI.Settings.Models;
using System.Windows;

namespace HunterPie.UI.Settings.Converter;

public interface IConfigurationViewBuilder
{
    public FrameworkElement Build(IConfigurationProperty property);
}