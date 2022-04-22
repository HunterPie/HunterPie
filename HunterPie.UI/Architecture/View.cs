using HunterPie.Core.Architecture;
using HunterPie.UI.Overlay;
using System;
using System.Windows.Controls;

namespace HunterPie.UI.Architecture
{
    public class View<TViewModel> : UserControl
        where TViewModel : Bindable
    {
        public TViewModel ViewModel => (TViewModel)DataContext;

        protected virtual TViewModel InitializeViewModel()
        {
            if (this is IWidgetWindow widget)
                return (TViewModel)Activator.CreateInstance(typeof(TViewModel), widget.Settings);

            return Activator.CreateInstance<TViewModel>();
        }

        public View()
        {
            DataContext = InitializeViewModel();
        }
    }
}
