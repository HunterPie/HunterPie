using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.Core.Game;
using HunterPie.Core.Game.Entity.Player;
using HunterPie.UI.Overlay.Widgets.Abnormality.ViewModel;
using System.Linq;
using System.Windows;
using System.Windows.Threading;

namespace HunterPie.UI.Overlay.Widgets.Abnormality;

internal class AbnormalityWidgetContextHandler : IContextHandler
{
    private readonly IContext _context;
    private readonly AbnormalityWidgetConfig _config;
    private readonly AbnormalityBarViewModel _viewModel;

    public AbnormalityWidgetContextHandler(
        IContext context,
        AbnormalityWidgetConfig config,
        AbnormalityBarViewModel viewModel)
    {
        _config = config;
        _context = context;

        _viewModel = viewModel;

        UpdateData();
        HookEvents();
    }

    public void HookEvents()
    {
        _context.Game.Player.OnAbnormalityStart += OnAbnormalityStart;
        _context.Game.Player.OnAbnormalityEnd += OnAbnormalityEnd;
    }

    private void OnAbnormalityEnd(object sender, IAbnormality e)
    {
        Application.Current.Dispatcher.BeginInvoke(() =>
        {
            AbnormalityContextHandler handler = _viewModel.Abnormalities.Cast<AbnormalityContextHandler>()
                .FirstOrDefault(vm => vm.Context == e);

            if (handler is null)
                return;

            handler.UnhookEvents();
            _viewModel.Abnormalities.Remove(handler);
        }, DispatcherPriority.Normal);
    }

    private void OnAbnormalityStart(object sender, IAbnormality e)
    {
        Application.Current.Dispatcher.BeginInvoke(() =>
        {
            if (!_config.AllowedAbnormalities.Contains(e.Id))
                return;

            _viewModel.Abnormalities.Add(new AbnormalityContextHandler(e));
        }, DispatcherPriority.Normal);
    }

    public void UnhookEvents()
    {
        _context.Game.Player.OnAbnormalityStart -= OnAbnormalityStart;
        _context.Game.Player.OnAbnormalityEnd -= OnAbnormalityEnd;
    }

    private void UpdateData()
    {
        Application.Current.Dispatcher.BeginInvoke(() =>
        {
            foreach (IAbnormality abnormality in _context.Game.Player.Abnormalities)
            {
                if (!_config.AllowedAbnormalities.Contains(abnormality.Id))
                    return;

                _viewModel.Abnormalities.Add(new AbnormalityContextHandler(abnormality));
            }
        }, DispatcherPriority.Normal);
    }
}