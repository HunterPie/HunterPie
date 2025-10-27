using HunterPie.Core.Architecture;
using HunterPie.Features.Settings.ViewModels;
using HunterPie.UI.Architecture.Bindings;
using HunterPie.UI.Architecture.Tree;
using HunterPie.UI.Controls.TextBox.Events;
using HunterPie.UI.Settings.Converter.Model;
using HunterPie.UI.Settings.ViewModels;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media.Animation;
using AppResources = HunterPie.UI.Assets.Application.Resources;

namespace HunterPie.Features.Settings.Views;
/// <summary>
/// Interaction logic for SettingsView.xaml
/// </summary>
public partial class SettingsView : UserControl
{
    private readonly PropertyCondition _defaultCondition = new PropertyCondition(
        Property: new Observable<bool>(true),
        Value: true
    );
    private readonly Storyboard _disableSettingComponentAnimation;
    private readonly Storyboard _enableSettingComponentAnimation;

    public SettingsView()
    {
        InitializeComponent();

        _disableSettingComponentAnimation = AppResources.Get<Storyboard>("Animations.Scale.Hide");
        _enableSettingComponentAnimation = AppResources.Get<Storyboard>("Animations.Scale.Show");
    }

    private void OnSearchTextChanged(object? sender, SearchTextChangedEventArgs e)
    {
        if (DataContext is not SettingsViewModel vm)
            return;

        vm.Search(e.Text);
    }

    private void OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (DataContext is not SettingsViewModel vm)
            return;

        vm.ChangeSettingsGroup();
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        if (DataContext is not SettingsViewModel vm)
            return;

        vm.FetchVersion();
    }

    private void OnRetryVersionFetchClick(object sender, RoutedEventArgs e)
    {
        if (DataContext is not SettingsViewModel vm)
            return;

        vm.FetchVersion();
    }

    private void OnDownloadVersionClick(object sender, RoutedEventArgs e)
    {
        if (DataContext is not SettingsViewModel vm)
            return;

        vm.ExecuteUpdate();
    }

    private void OnTitleClick(object sender, MouseButtonEventArgs e)
    {
        e.Handled = true;
    }

    private void OnSettingPropertyLoaded(object sender, RoutedEventArgs e)
    {
        if (sender is not FrameworkElement { DataContext: ConfigurationPropertyViewModel vm } element)
            return;

        var dataTrigger = new MultiDataTrigger();

        IReadOnlyCollection<PropertyCondition> conditions = vm.Conditions.Count > 0
            ? vm.Conditions
            : new[] { _defaultCondition };

        foreach (PropertyCondition condition in conditions)
        {
            BindingBase binding = Binder.Create(condition.Property);

            dataTrigger.Conditions.Add(
                item: new Condition(
                    binding: binding,
                    conditionValue: condition.Value
                )
            );
        }

        dataTrigger.EnterActions.Add(
            value: new BeginStoryboard
            {
                Storyboard = _enableSettingComponentAnimation
            }
        );
        dataTrigger.ExitActions.Add(
            value: new BeginStoryboard
            {
                Storyboard = _disableSettingComponentAnimation
            }
        );

        var style = new Style(element.GetType());

        style.Triggers.Add(dataTrigger);

        style.Setters.Add(new Setter(OpacityProperty, 0.0));

        element.Style = style;
    }

    private void OnSizeChanged(object sender, SizeChangedEventArgs e)
    {
        if (sender is not FrameworkElement element)
            return;

        if (element.TryFindParent<Border>() is not { } parent)
            return;

        bool hasChildrenVisible = e.NewSize.Height > 0;

        parent.Visibility = hasChildrenVisible
            ? Visibility.Visible
            : Visibility.Collapsed;
    }
}