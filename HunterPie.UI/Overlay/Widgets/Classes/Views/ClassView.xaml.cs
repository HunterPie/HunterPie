using HunterPie.Core.Client.Configuration.Overlay.Class;
using HunterPie.Core.Settings;
using HunterPie.UI.Architecture;
using HunterPie.UI.Overlay.Enums;
using HunterPie.UI.Overlay.Widgets.Classes.ViewModels;
using System;

namespace HunterPie.UI.Overlay.Widgets.Classes.Views;

/// <summary>
/// Interaction logic for ClassView.xaml
/// </summary>
public partial class ClassView : View<ClassViewModel>, IWidget<ClassWidgetConfig>, IWidgetWindow
{
    public WidgetType Type => WidgetType.ClickThrough;
    IWidgetSettings IWidgetWindow.Settings => Settings;
    public event EventHandler<WidgetType> OnWidgetTypeChange;
    public ClassWidgetConfig Settings => ViewModel.CurrentSettings;
    public string Title => "Class Widget";


    public ClassView()
    {
        InitializeComponent();
    }
}