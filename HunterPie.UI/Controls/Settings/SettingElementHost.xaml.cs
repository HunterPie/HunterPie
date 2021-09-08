using HunterPie.UI.Controls.Buttons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;

namespace HunterPie.UI.Controls.Settings
{
    /// <summary>
    /// Interaction logic for SettingElementHost.xaml
    /// </summary>
    public partial class SettingElementHost : UserControl
    {
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(SettingElementHost), new PropertyMetadata("UNKNOWN_STRING"));

        public string Description
        {
            get { return (string)GetValue(DescriptionProperty); }
            set { SetValue(DescriptionProperty, value); }
        }
        public static readonly DependencyProperty DescriptionProperty =
            DependencyProperty.Register("Description", typeof(string), typeof(SettingElementHost), new PropertyMetadata("UNKNOWN_STRING_DESC"));

        public UIElement Hosted
        {
            get { return (UIElement)GetValue(HostedProperty); }
            set { SetValue(HostedProperty, value); }
        }
        public static readonly DependencyProperty HostedProperty =
            DependencyProperty.Register("Hosted", typeof(UIElement), typeof(SettingElementHost));

        public SettingElementHost()
        {
            InitializeComponent();
            DataContext = this;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (!IsMouseOver)
                return;

            Point pos = e.GetPosition(this);

            double left = pos.X - (PART_Highlight.ActualWidth / 2);
            double top = pos.Y - (PART_Highlight.ActualHeight / 2);

            PART_Highlight.Margin = new Thickness(left, top, 0, 0);
        }


    }
}
