using HunterPie.Core.API;
using HunterPie.Core.API.Schemas;
using HunterPie.Core.Architecture;
using HunterPie.Core.Client;
using System;
using System.Reflection;
using System.Security.Principal;
using System.Windows;

namespace HunterPie.GUI.Parts
{
    public class HeaderBarViewModel : Bindable
    {

        private bool _isSupporter;
        private bool _isFetchingSupporter;

        public string Version
        {
            get
            {
                Assembly self = typeof(App).Assembly;
                AssemblyName name = self.GetName();
                Version ver = name.Version;

                return $"v{ver}";
            }
        }

        public bool IsSupporter { get => _isSupporter; set { SetValue(ref _isSupporter, value); } }
        public bool IsFetchingSupporter { get => _isFetchingSupporter; set { SetValue(ref _isFetchingSupporter, value); } }

        public Visibility IsRunningAsAdmin => GetAdminState()
                                              ? Visibility.Visible
                                              : Visibility.Collapsed;

        public void MinimizeApplication()
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;

            if (ClientConfig.Config.Client.MinimizeToSystemTray)
            {
                Application.Current.MainWindow.Hide();
            }
        }

        public async void FetchSupporterStatus()
        {
            IsFetchingSupporter = true;

            SupporterValidationResSchema res = await PoogieApi.ValidateSupporterToken();

            IsSupporter = res?.IsValid ?? false;

            IsFetchingSupporter = false;
        }

        public void CloseApplication()
        {
            Application.Current.MainWindow.Close();
        }
        
        public void DragApplication()
        {
            Application.Current.MainWindow.DragMove();
        }

        private bool GetAdminState()
        {
            WindowsIdentity winIdentity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(winIdentity);
            
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }
    }
}
