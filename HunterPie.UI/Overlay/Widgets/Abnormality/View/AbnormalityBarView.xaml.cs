using HunterPie.Core.Client;
using HunterPie.Core.Settings;
using HunterPie.UI.Architecture;
using HunterPie.UI.Overlay.Widgets.Abnormality.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace HunterPie.UI.Overlay.Widgets.Abnormality.View
{
    /// <summary>
    /// Interaction logic for AbnormalityBarView.xaml
    /// </summary>
    public partial class AbnormalityBarView : View<AbnormalityBarViewModel>, IWidget
    {
        public AbnormalityBarView()
        {
            InitializeComponent();
        }

        public IWidgetSettings Settings => ClientConfig.Config.Overlay.AbnormalityWidget;

        public string Title => "Abnormality Widget";

        #region Mock
        private void View_Loaded(object sender, RoutedEventArgs e)
        {
            List<AbnormalityViewModel> vms = new()
            {
                new()
                {
                    Icon = "ICON_ATTACKUP",
                    Timer = 100.0,
                    MaxTimer = 100.0,
                    IsBuff = true
                },
                new()
                {
                    Icon = "ICON_EFFLUVIA",
                    Timer = 100.0,
                    MaxTimer = 200.0,
                    IsBuff = false
                },
                new()
                {
                    Icon = "ICON_NATURALHEALING",
                    Timer = 300.0,
                    MaxTimer = 420.0,
                    IsBuff = true
                },
                new()
                {
                    Icon = "ICON_ALLRESUP",
                    Timer = 100.0,
                    MaxTimer = 200.0,
                    IsBuff = true
                }
            };

            foreach (var abnorm in vms)
                ViewModel.Abnormalities.Add(abnorm);
            
            var updater = new DispatcherTimer()
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            updater.Tick += (_, __) =>
            {
                foreach (var abnorm in ViewModel.Abnormalities)
                {
                    if (abnorm.Timer <= 0.0)
                        abnorm.Timer = abnorm.MaxTimer;

                    abnorm.Timer -= 1.0;
                }
                    
            };
            updater.Start();
        }
        #endregion
    }
}
