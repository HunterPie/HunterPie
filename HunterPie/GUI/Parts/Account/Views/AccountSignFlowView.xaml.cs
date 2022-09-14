using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Extensions;
using System;
using System.Windows;
using System.Windows.Controls;

namespace HunterPie.GUI.Parts.Account.Views
{
    /// <summary>
    /// Interaction logic for AccountSignFlowView.xaml
    /// </summary>
    public partial class AccountSignFlowView : UserControl, IEventDispatcher
    {
        public event EventHandler<EventArgs> OnFormClose;

        public AccountSignFlowView()
        {
            InitializeComponent();
        }

        private void OnCloseClick(object sender, RoutedEventArgs e) => this.Dispatch(OnFormClose);
    }
}
