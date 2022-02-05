using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.UI.Controls.Settings.Custom.Abnormality;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public AbnormalityWidgetConfig Config => (AbnormalityWidgetConfig)DataContext;

        public AbnormalityWidgetConfigWindow()
        {
            InitializeComponent();

            LoadAllAbnormalities();
        }

        private void LoadAllAbnormalities()
        {
            // TODO: Refactor this to make it not in the view...
            XmlDocument document = new();
            document.Load(ClientInfo.GetPathFor("Game/Data/Rise/AbnormalityData.xml"));
            XmlNodeList nodes = document.SelectNodes("/Abnormality");

            foreach (XmlNode node in nodes)
            {
                string category = node.ParentNode.Name;
                string name = node.Attributes["Name"]?.Value ?? "ABNORMALITY_UNKNOWN";
                string icon = node.Attributes["Icon"]?.Value ?? "ICON_MISSING";
                string id = node.Attributes["Id"].Value;

                if (!collections.ContainsKey(category))
                    collections.Add(category, new() { Name = category, Icon = "ICON_SETTINGS" });

                AbnormalityCollectionViewModel collection = collections[category];
                
                collection.Abnormalities.Add(new()
                {
                    Name = name,
                    Icon = icon,
                    IsEnabled = Config.AllowedAbnormalities.Contains($"{category}_{id}")
                });

            }
        }
    }
}
