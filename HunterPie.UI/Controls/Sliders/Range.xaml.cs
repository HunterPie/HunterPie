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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HunterPie.UI.Controls.Sliders
{
    /// <summary>
    /// Interaction logic for Range.xaml
    /// </summary>
    public partial class Range : UserControl
    {

        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(double), typeof(Range), new PropertyMetadata(0.0));
        
        public double Maximum
        {
            get { return (double)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }
        public static readonly DependencyProperty MaximumProperty =
            DependencyProperty.Register("Maximum", typeof(double), typeof(Range), new PropertyMetadata(0.0));

        public double Minimum
        {
            get { return (double)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }
        public static readonly DependencyProperty MinimumProperty =
            DependencyProperty.Register("Minimum", typeof(double), typeof(Range), new PropertyMetadata(0.0));

        public double Change
        {
            get { return (double)GetValue(ChangeProperty); }
            set { SetValue(ChangeProperty, value); }
        }
        public static readonly DependencyProperty ChangeProperty =
            DependencyProperty.Register("Change", typeof(double), typeof(Range), new PropertyMetadata(1.0));

        public Range()
        {
            InitializeComponent();
            DataContext = this;
        }
    }
}
