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
