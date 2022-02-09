using HunterPie.UI.Architecture;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace HunterPie.UI.Controls.Buttons
{
    /// <summary>
    /// Interaction logic for NativeButton.xaml
    /// </summary>
    public partial class Button : ClickableControl
    {
        private readonly Storyboard _rippleAnimation;

        public new object Content
        {
            get { return (object)GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }
        public static new readonly DependencyProperty ContentProperty =
            DependencyProperty.Register("Content", typeof(object), typeof(Button));

        public new Brush Foreground
        {
            get { return (Brush)GetValue(ForegroundProperty); }
            set { SetValue(ForegroundProperty, value); }
        }
        public static new readonly DependencyProperty ForegroundProperty =
            DependencyProperty.Register("Foreground", typeof(Brush), typeof(Button), new PropertyMetadata(Brushes.WhiteSmoke));

        public new VerticalAlignment VerticalContentAlignment
        {
            get { return (VerticalAlignment)GetValue(VerticalContentAlignmentProperty); }
            set { SetValue(VerticalContentAlignmentProperty, value); }
        }

        public static new readonly DependencyProperty VerticalContentAlignmentProperty =
            DependencyProperty.Register("VerticalContentAlignment", typeof(VerticalAlignment), typeof(Button), new PropertyMetadata(VerticalAlignment.Center));

        public new HorizontalAlignment HorizontalContentAlignment
        {
            get { return (HorizontalAlignment)GetValue(HorizontalContentAlignmentProperty); }
            set { SetValue(HorizontalContentAlignmentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HorizontalContentAlignment.  This enables animation, styling, binding, etc...
        public static new readonly DependencyProperty HorizontalContentAlignmentProperty =
            DependencyProperty.Register("HorizontalContentAlignment", typeof(HorizontalAlignment), typeof(Button), new PropertyMetadata(HorizontalAlignment.Center));

        public Button()
        {
            InitializeComponent();

            _rippleAnimation = FindResource("PART_RippleAnimation") as Storyboard;
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
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
