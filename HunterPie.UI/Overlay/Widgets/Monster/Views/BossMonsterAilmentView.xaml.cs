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

namespace HunterPie.UI.Overlay.Widgets.Monster.Views
{
    /// <summary>
    /// Interaction logic for BossMonsterAilmentView.xaml
    /// </summary>
    public partial class BossMonsterAilmentView : UserControl
    {
        public double Current
        {
            get { return (double)GetValue(CurrentProperty); }
            set { SetValue(CurrentProperty, value); }
        }
        public static readonly DependencyProperty CurrentProperty =
            DependencyProperty.Register("Current", typeof(double), typeof(BossMonsterAilmentView), new PropertyMetadata(0.0));

        public double Max
        {
            get { return (double)GetValue(MaxProperty); }
            set { SetValue(MaxProperty, value); }
        }
        public static readonly DependencyProperty MaxProperty =
            DependencyProperty.Register("Max", typeof(double), typeof(BossMonsterAilmentView), new PropertyMetadata(0.0));

        public BossMonsterAilmentView()
        {
            InitializeComponent();
        }

    }
}
