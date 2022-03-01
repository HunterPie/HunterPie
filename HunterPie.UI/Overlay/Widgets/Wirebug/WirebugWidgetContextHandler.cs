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
        private readonly static HashSet<int> UnavailableStages = new() { 
            1, // Room
            3, // Gathering Hub
            4, // Hub Prep Plaza
        };
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
            Player.OnStageUpdate += OnStageUpdate;
            Player.OnWirebugsRefresh += OnWirebugsRefresh;
        }

        public void UnhookEvents()
        {
            Player.OnStageUpdate -= OnStageUpdate;
            Player.OnWirebugsRefresh -= OnWirebugsRefresh;
        }


        private void OnStageUpdate(object sender, EventArgs e) => ViewModel.IsAvailable = !UnavailableStages.Contains(Player.StageId);

        private void OnWirebugsRefresh(object sender, MHRWirebug[] e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                foreach (WirebugViewModel vm in ViewModel.Elements)
                    if (vm is WirebugContextHandler model)
                        model.UnhookEvents();

                ViewModel.Elements.Clear();

                foreach (MHRWirebug wirebug in e)
                    ViewModel.Elements.Add(new WirebugContextHandler(wirebug));
            });
        }

        private void UpdateData()
        {
            ViewModel.IsAvailable = !UnavailableStages.Contains(Player.StageId);

            foreach (MHRWirebug wirebug in Player.Wirebugs)
                ViewModel.Elements.Add(new WirebugContextHandler(wirebug));
        }
    }
}
