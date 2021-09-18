using HunterPie.Core.Logger;
using HunterPie.Core.Settings.Types;
using HunterPie.UI.Architecture;
using HunterPie.UI.Architecture.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HunterPie.UI.Controls.Buttons
{
    /// <summary>
    /// Interaction logic for Switch.xaml
    /// </summary>
    public partial class Switch : ClickableControl
    {
        public bool IsActive
        {
            get { return (bool)GetValue(IsActiveProperty); }
            set { SetValue(IsActiveProperty, value); }
        }
        public static readonly DependencyProperty IsActiveProperty =
            DependencyProperty.Register("IsActive", typeof(bool), typeof(Switch), new PropertyMetadata(false));

        public Switch()
        {
            InitializeComponent();
        }

        protected override void OnClickEvent()
        {
            base.OnClickEvent();

            IsActive = !IsActive;
        }
    }
}
