using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.UI.Controls.Settings.Custom.Abnormality;
using HunterPie.UI.Controls.Settings.ViewModel;
using HunterPie.UI.Settings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Localization = HunterPie.Core.Client.Localization.Localization;
using System.Windows;
using System.Xml;
using HunterPie.UI.Controls.TextBox.Events;
using System.Text.RegularExpressions;

namespace HunterPie.UI.Controls.Settings.Custom
{
    /// <summary>
    /// Interaction logic for AbnormalityWidgetConfig.xaml
    /// </summary>
    public partial class AbnormalityWidgetConfigWindow : Window, INotifyPropertyChanged
    {
        private AbnormalityCollectionViewModel _selectedElement;

        private readonly Dictionary<string, AbnormalityCollectionViewModel> collections = new();
        public ObservableCollection<AbnormalityCollectionViewModel> Collections { get; } = new();
        public ObservableCollection<ISettingElementType> Elements { get; } = new();
        private Dictionary<string, object> _categoryIcons = new();

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

        public AbnormalityWidgetConfigWindow(AbnormalityWidgetConfig config)
        {
            Config = config;
            DataContext = config;
            InitializeComponent();

            LoadIcons();
            BuildVisualConfig();
            LoadAllAbnormalities();
        }

        private void LoadIcons()
        {
            _categoryIcons.Add("Songs", FindResource("ICON_SELFIMPROVEMENT"));
            _categoryIcons.Add("Consumables", FindResource("ITEM_DEMONDRUG"));
            _categoryIcons.Add("Debuffs", FindResource("ICON_VENOM"));
            _categoryIcons.Add("Skills", FindResource("ICON_ARMORSKILL_WHITE"));
        }

        private void BuildVisualConfig()
        {

            foreach (ISettingElementType element in VisualConverterManager.BuildSubElements(Config))
                Elements.Add(element);
        }

        private void LoadAllAbnormalities()
        {
            // TODO: Refactor this to make it not in the view...
            XmlDocument document = new();
            document.Load(ClientInfo.GetPathFor("Game/Rise/Data/AbnormalityData.xml"));
            XmlNodeList nodes = document.SelectNodes("//Abnormalities/*/Abnormality");

            foreach (XmlNode node in nodes)
            {
                string category = node.ParentNode.Name;
                string categoryString = $"//Strings/Client/Settings/Setting[@Id='ABNORMALITY_{category.ToUpperInvariant()}_STRING']";
                string name = node.Attributes["Name"]?.Value ?? "ABNORMALITY_UNKNOWN";
                string icon = node.Attributes["Icon"]?.Value ?? "ICON_MISSING";
                string id = node.Attributes["Id"].Value;
                string abnormId = $"{category}_{id}";

                if (!collections.ContainsKey(category))
                    collections.Add(category, new() 
                        { 
                            Name = Localization.QueryString(categoryString),
                            Description = Localization.QueryDescription(categoryString),
                            Icon = _categoryIcons[category]
                        });

                AbnormalityCollectionViewModel collection = collections[category];
                
                collection.Abnormalities.Add(new()
                {
                    Name = Localization.QueryString($"//Strings/Abnormalities/Abnormality[@Id='{name}']"),
                    Icon = icon,
                    Id = abnormId,
                    Category = Localization.QueryString(categoryString),
                    IsMatch = true,
                    IsEnabled = Config.AllowedAbnormalities.Contains(abnormId)
                });

            }

            foreach (AbnormalityCollectionViewModel coll in collections.Values)
                Collections.Add(coll);
        }

        private void OnClosing(object sender, CancelEventArgs e)
        {
            Config.AllowedAbnormalities.Clear();

            foreach (AbnormalityCollectionViewModel collection in Collections)
                foreach (var abnorm in collection.Abnormalities.Where(a => a.IsEnabled))
                    Config.AllowedAbnormalities.Add(abnorm.Id);
        }

        private void OnSearchTextChanged(object sender, SearchTextChangedEventArgs e)
        {
            if (SelectedCollection is not null)
            {
                foreach (AbnormalityViewModel vm in SelectedCollection.Abnormalities)
                {
                    vm.IsMatch = string.IsNullOrEmpty(e.Text) || Regex.IsMatch(vm.Name, e.Text, RegexOptions.IgnoreCase);
                }
            }
        }

        private void OnSelectAllClick(object sender, EventArgs e)
        {
            if (SelectedCollection is not null)
            {
                foreach (AbnormalityViewModel vm in SelectedCollection.Abnormalities)
                    vm.IsEnabled = true;
            }
        }
    }
}
