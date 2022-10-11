using HunterPie.Core.Architecture;
using HunterPie.UI.Overlay;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace HunterPie.UI.Architecture;

public class View<TViewModel> : UserControl
    where TViewModel : Bindable
{
    public TViewModel ViewModel => (TViewModel)DataContext;

    protected virtual TViewModel InitializeViewModel(params object[] args)
    {
        if (this is IWidgetWindow)
        {
            try
            {
                return (TViewModel)Activator.CreateInstance(typeof(TViewModel), args);
            }
            catch { }
        };

        return Activator.CreateInstance<TViewModel>();
    }
    protected bool IsDesignMode => DesignerProperties.GetIsInDesignMode(this);

    public View()
    {
        DataContext = InitializeViewModel();
    }

    public View(params object[] args)
    {
        DataContext = InitializeViewModel(args);
    }

    protected override void OnInitialized(EventArgs e)
    {
        base.OnInitialized(e);

        if (IsDesignMode)
            return;

        Unloaded += OnViewUnloaded;

        Initialize();
    }

    private void OnViewUnloaded(object sender, RoutedEventArgs e)
    {
        if (ViewModel is IDisposable vm)
            vm.Dispose();

        Unloaded -= OnViewUnloaded;
    }

    protected virtual void Initialize() { }
}
