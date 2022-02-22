using HunterPie.Core.Architecture;
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
            return Activator.CreateInstance<TViewModel>();
        }

        public View()
        {
            DataContext = InitializeViewModel();
        }
    }
}
