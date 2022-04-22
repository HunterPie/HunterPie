using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.Core.Game.Rise;
using HunterPie.Core.Game.Rise.Entities;
using HunterPie.UI.Overlay.Widgets.Wirebug.ViewModel;
using HunterPie.UI.Overlay.Widgets.Wirebug.Views;
using System;
using System.Collections.Generic;

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
        private readonly WirebugsView View;
        private MHRPlayer Player => (MHRPlayer)Context.Game.Player;
        

        public WirebugWidgetContextHandler(MHRContext context)
        {
            View = new WirebugsView(ClientConfig.Config.Rise.Overlay.WirebugWidget);
            WidgetManager.Register<WirebugsView, WirebugWidgetConfig>(View);
            
            ViewModel = View.ViewModel;
            Context = context;

            HookEvents();
            UpdateData();
        }

        public void HookEvents()
        {
            Player.OnStageUpdate += OnStageUpdate;
            Player.OnWirebugsRefresh += OnWirebugsRefresh;
        }

        public void UnhookEvents()
        {
            Player.OnStageUpdate -= OnStageUpdate;
            Player.OnWirebugsRefresh -= OnWirebugsRefresh;
            WidgetManager.Unregister<WirebugsView, WirebugWidgetConfig>(View);
        }


        private void OnStageUpdate(object sender, EventArgs e) => ViewModel.IsAvailable = !UnavailableStages.Contains(Player.StageId);

        private void OnWirebugsRefresh(object sender, MHRWirebug[] e)
        {
            View.Dispatcher.Invoke(() =>
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
