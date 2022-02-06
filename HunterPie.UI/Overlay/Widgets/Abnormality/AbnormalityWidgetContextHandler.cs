using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.Core.Game;
using HunterPie.Core.Game.Client;
using HunterPie.UI.Overlay.Widgets.Abnormality.View;
using HunterPie.UI.Overlay.Widgets.Abnormality.ViewModel;
using System.Linq;
using System.Windows;
using System.Windows.Threading;

namespace HunterPie.UI.Overlay.Widgets.Abnormality
{
    class AbnormalityWidgetContextHandler : IContextHandler
    {

        public readonly Context Context;
        public readonly AbnormalityBarViewModel ViewModel;
        private readonly int Index;
        private AbnormalityWidgetConfig Config => ClientConfig.Config.Overlay.AbnormalityTray.Trays[Index];

        public AbnormalityWidgetContextHandler(Context context, int index)
        {
            Index = index;
            var widget = new AbnormalityBarView(index);
            WidgetManager.Register(widget);
            Context = context;

            ViewModel = (AbnormalityBarViewModel)widget.DataContext;

            UpdateData();
            HookEvents();
        }

        private void HookEvents()
        {
            Context.Game.Player.OnAbnormalityStart += OnAbnormalityStart;
            Context.Game.Player.OnAbnormalityEnd += OnAbnormalityEnd;
        }

        private void OnAbnormalityEnd(object sender, IAbnormality e)
        {
            Application.Current.Dispatcher.InvokeAsync(() =>
            {
                AbnormalityContextHandler handler = ViewModel.Abnormalities.Cast<AbnormalityContextHandler>()
                    .FirstOrDefault(vm => vm.Context == e);

                if (handler is null)
                    return;

                handler.UnhookEvents();
                ViewModel.Abnormalities.Remove(handler);
            }, DispatcherPriority.Normal);
        }

        private void OnAbnormalityStart(object sender, IAbnormality e)
        {
            Application.Current.Dispatcher.InvokeAsync(() =>
            {
                if (!Config.AllowedAbnormalities.Contains(e.Id))
                    return;

                ViewModel.Abnormalities.Add(new AbnormalityContextHandler(e));
            }, DispatcherPriority.Normal);
        }

        public void UnhookEvents()
        {
            Context.Game.Player.OnAbnormalityStart -= OnAbnormalityStart;
            Context.Game.Player.OnAbnormalityEnd -= OnAbnormalityEnd;
        }

        private void UpdateData()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                foreach (IAbnormality abnormality in Context.Game.Player.Abnormalities)
                {
                    if (!Config.AllowedAbnormalities.Contains(abnormality.Id))
                        return;

                    ViewModel.Abnormalities.Add(new AbnormalityContextHandler(abnormality));
                }
            }, DispatcherPriority.Normal);
        }
    }
}
