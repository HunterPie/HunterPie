using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.Core.Game.Rise;
using HunterPie.Core.Game.Rise.Entities;
using HunterPie.UI.Overlay.Widgets.Wirebug.ViewModel;
using HunterPie.UI.Overlay.Widgets.Wirebug.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HunterPie.UI.Overlay.Widgets.Wirebug
{
    public class WirebugWidgetContextHandler : IContextHandler
    {

        private readonly MHRContext Context;
        private readonly WirebugsViewModel ViewModel;
        private MHRPlayer Player => (MHRPlayer)Context.Game.Player;
        

        public WirebugWidgetContextHandler(MHRContext context)
        {
            var widget = new WirebugsView();
            WidgetManager.Register<WirebugsView, WirebugWidgetConfig>(widget);
            
            ViewModel = widget.ViewModel;
            Context = context;

            HookEvents();
            UpdateData();
        }

        private void HookEvents()
        {
            Player.OnWirebugsRefresh += OnWirebugsRefresh;
        }

        public void UnhookEvents()
        {
            Player.OnWirebugsRefresh -= OnWirebugsRefresh;
        }

        private void OnWirebugsRefresh(object sender, MHRWirebug[] e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                foreach (WirebugContextHandler vm in ViewModel.Elements.Cast<WirebugContextHandler>())
                    vm.UnhookEvents();

                ViewModel.Elements.Clear();

                foreach (MHRWirebug wirebug in e)
                    ViewModel.Elements.Add(new WirebugContextHandler(wirebug));
            });
        }

        private void UpdateData()
        {
            foreach (MHRWirebug wirebug in Player.Wirebugs)
                ViewModel.Elements.Add(new WirebugContextHandler(wirebug));
        }
    }
}
