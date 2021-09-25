using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace HunterPie.UI.Overlay.Controls
{
    /// <summary>
    /// Interaction logic for Abnormality.xaml
    /// </summary>
    public partial class Abnormality : UserControl
    {
        public bool IsBuff
        {
            get { return (bool)GetValue(IsBuffProperty); }
            set { SetValue(IsBuffProperty, value); }
        }
        public static readonly DependencyProperty IsBuffProperty =
            DependencyProperty.Register("IsBuff", typeof(bool), typeof(Abnormality), new PropertyMetadata(false));

        public double Time
        {
            get { return (double)GetValue(TimeProperty); }
            set { SetValue(TimeProperty, value); }
        }
        public static readonly DependencyProperty TimeProperty =
            DependencyProperty.Register("Time", typeof(double), typeof(Abnormality), new PropertyMetadata(0.0));

        public double MaxTime
        {
            get { return (double)GetValue(MaxTimeProperty); }
            set { SetValue(MaxTimeProperty, value); }
        }
        public static readonly DependencyProperty MaxTimeProperty =
            DependencyProperty.Register("MaxTime", typeof(double), typeof(Abnormality), new PropertyMetadata(0.0));

        public ImageSource Icon
        {
            get { return (ImageSource)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof(ImageSource), typeof(Abnormality));

        public Abnormality()
        {
            InitializeComponent();
        }
    }
}
