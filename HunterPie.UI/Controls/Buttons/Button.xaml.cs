using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace HunterPie.UI.Controls.Buttons
{
    /// <summary>
    /// Interaction logic for NativeButton.xaml
    /// </summary>
    public partial class Button : UserControl
    {
        private readonly Storyboard _rippleAnimation;
        private bool _isMouseInside;
        private bool _isMouseDown;

        public event EventHandler<EventArgs> OnButtonClick;

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(Button), new PropertyMetadata("Button"));

        public new Brush Foreground
        {
            get { return (Brush)GetValue(ForegroundProperty); }
            set { SetValue(ForegroundProperty, value); }
        }
        public static new readonly DependencyProperty ForegroundProperty =
            DependencyProperty.Register("Foreground", typeof(Brush), typeof(Button), new PropertyMetadata(Brushes.WhiteSmoke));

        public Button()
        {
            InitializeComponent();

            _rippleAnimation = FindResource("PART_RippleAnimation") as Storyboard;
        }

        private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _isMouseDown = true;
            var targetWidth = Math.Max(ActualWidth, ActualHeight) * 2;
            var mousePosition = e.GetPosition(this);
            var startMargin = new Thickness(mousePosition.X, mousePosition.Y, 0, 0);
            PART_Ripple.Margin = startMargin;
            (_rippleAnimation.Children[0] as DoubleAnimation).To = targetWidth;
            (_rippleAnimation.Children[1] as ThicknessAnimation).From = startMargin;
            (_rippleAnimation.Children[1] as ThicknessAnimation).To = new Thickness(mousePosition.X - targetWidth / 2, mousePosition.Y - targetWidth / 2, 0, 0);
            PART_Ripple.BeginStoryboard(_rippleAnimation);

            e.Handled = true;
        }

        private void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            // Was a click!
            if (_isMouseDown && _isMouseInside)
                OnButtonClick?.Invoke(this, e);

            _isMouseDown = false;
        }

        private void OnMouseEnter(object sender, MouseEventArgs e) => _isMouseInside = true;
        private void OnMouseLeave(object sender, MouseEventArgs e)
        {
            _isMouseInside = false;
            _isMouseDown = false;
        }
    }
}
