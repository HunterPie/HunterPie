using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.UI.Architecture.Navigator;
using HunterPie.UI.Controls.Settings.Custom.Abnormality;
using HunterPie.UI.Controls.Settings.ViewModel;
using HunterPie.UI.Controls.TextBox.Events;
using HunterPie.UI.Settings;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace HunterPie.UI.Controls.Settings.Custom;
/// <summary>
/// Interaction logic for AbnormalityWidgetConfigView.xaml
/// </summary>
public partial class AbnormalityWidgetConfigView : UserControl, INotifyPropertyChanged
{
    // TODO: Separate View from ViewModel

    private AbnormalityCollectionViewModel _selectedElement;

    public ObservableCollection<AbnormalityCollectionViewModel> Collections { get; } = new();
    public ObservableCollection<ISettingElementType> Elements { get; } = new();

    public AbnormalityCollectionViewModel SelectedCollection
    {
        get => _selectedElement;
        set
        {
            if (value != _selectedElement)
            {
                _selectedElement = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedCollection)));
            }
        }
    }

    public readonly AbnormalityWidgetConfig Config;

    public event PropertyChangedEventHandler PropertyChanged;

    public AbnormalityWidgetConfigView(AbnormalityWidgetConfig config)
    {
        Config = config;
        DataContext = config;
        InitializeComponent();

        BuildVisualConfig();
        LoadAbnormalities();
    }


    private void BuildVisualConfig()
    {
        foreach (ISettingElementType element in VisualConverterManager.BuildSubElements(Config))
            Elements.Add(element);
    }

    private void LoadAbnormalities()
    {
        Collections.Clear();

        AbnormalityCollectionViewModel[] collections = AbnormalitiesViewHelper.GetViewModelsBy(
            ClientConfig.Config.Client.LastConfiguredGame.Value,
            Config
        );

        foreach (AbnormalityCollectionViewModel coll in collections)
            Collections.Add(coll);
    }

    private void OnSearchTextChanged(object sender, SearchTextChangedEventArgs e)
    {
        if (SelectedCollection is null)
            return;

        foreach (AbnormalityViewModel vm in SelectedCollection.Abnormalities)
            vm.IsMatch = string.IsNullOrEmpty(e.Text) || Regex.IsMatch(vm.Name, e.Text, RegexOptions.IgnoreCase);
    }

    private void OnSelectAllClick(object sender, EventArgs e)
    {
        if (SelectedCollection is null)
            return;

        foreach (AbnormalityViewModel vm in SelectedCollection.Abnormalities)
            vm.IsEnabled = true;
    }

    private void OnBackButtonClick(object sender, RoutedEventArgs e)
    {
        Navigator.Return();
    }

    private void SaveAbnormalities()
    {
        Config.AllowedAbnormalities.Clear();

        foreach (AbnormalityCollectionViewModel collection in Collections)
            foreach (AbnormalityViewModel abnorm in collection.Abnormalities.Where(a => a.IsEnabled))
                Config.AllowedAbnormalities.Add(abnorm.Id);
    }

    private void OnUnload(object sender, RoutedEventArgs e)
    {
        SaveAbnormalities();
    }
}
