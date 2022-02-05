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
using System.Windows;
using System.Xml;

namespace HunterPie.UI.Controls.Settings.Custom
{
    /// <summary>
    /// Interaction logic for AbnormalityWidgetConfig.xaml
    /// </summary>
    public partial class AbnormalityWidgetConfigWindow : Window
    {

        private readonly Dictionary<string, AbnormalityCollectionViewModel> collections = new();
        public ObservableCollection<AbnormalityCollectionViewModel> Collections { get; } = new();
        public ObservableCollection<ISettingElementType> Elements { get; } = new();

        public readonly AbnormalityWidgetConfig Config;

        public AbnormalityWidgetConfigWindow(AbnormalityWidgetConfig config)
        {
            Config = config;
            DataContext = config;
            InitializeComponent();

            BuildVisualConfig();
            LoadAllAbnormalities();
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
                string name = node.Attributes["Name"]?.Value ?? "ABNORMALITY_UNKNOWN";
                string icon = node.Attributes["Icon"]?.Value ?? "ICON_MISSING";
                string id = node.Attributes["Id"].Value;
                string abnormId = $"{category}_{id}";

                if (!collections.ContainsKey(category))
                    collections.Add(category, new() { Name = category, Icon = FindResource("ICON_SELFIMPROVEMENT") });

                AbnormalityCollectionViewModel collection = collections[category];
                
                collection.Abnormalities.Add(new()
                {
                    Name = name,
                    Icon = icon,
                    Id = abnormId,
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
    }
}
