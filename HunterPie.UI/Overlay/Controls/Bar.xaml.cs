using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace HunterPie.UI.Overlay.Controls
{
    /// <summary>
    /// Interaction logic for Bar.xaml
    /// </summary>
    public partial class Bar : UserControl
    {

        public double ActualValueDelayed
        {
            get { return (double)GetValue(ActualValueDelayedProperty); }
            set { SetValue(ActualValueDelayedProperty, value); }
        }
        public static readonly DependencyProperty ActualValueDelayedProperty =
            DependencyProperty.Register("ActualValueDelayed", typeof(double), typeof(Bar));

        public double ActualValue
        {
            get { return (double)GetValue(ActualValueProperty); }
            set { SetValue(ActualValueProperty, value); }
        }
        public static readonly DependencyProperty ActualValueProperty =
            DependencyProperty.Register("ActualValue", typeof(double), typeof(Bar));

        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(double), typeof(Bar), new PropertyMetadata(0.0, OnValueChange));

        public double MaxValue
        {
            get { return (double)GetValue(MaxValueProperty); }
            set { SetValue(MaxValueProperty, value); }
        }

        public static readonly DependencyProperty MaxValueProperty =
            DependencyProperty.Register("MaxValue", typeof(double), typeof(Bar));

        public Brush ForegroundDelayed
        {
            get { return (Brush)GetValue(ForegroundDelayedProperty); }
            set { SetValue(ForegroundDelayedProperty, value); }
        }
        public static readonly DependencyProperty ForegroundDelayedProperty =
            DependencyProperty.Register("ForegroundDelayed", typeof(Brush), typeof(Bar));

        public Visibility MarkersVisibility
        {
            get { return (Visibility)GetValue(MarkersVisibilityProperty); }
            set { SetValue(MarkersVisibilityProperty, value); }
        }
        public static readonly DependencyProperty MarkersVisibilityProperty =
            DependencyProperty.Register("MarkersVisibility", typeof(Visibility), typeof(Bar), new PropertyMetadata(Visibility.Visible));

        public Bar()
        {
            InitializeComponent();
        }

        private static void OnValueChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Bar owner = d as Bar;

            if ((double)e.NewValue == 0.0 || owner.MaxValue == 0.0)
                return;

            double oldValue = owner.ActualWidth * ((double)e.OldValue / owner.MaxValue) - 4;
            double newValue = owner.ActualWidth * ((double)e.NewValue / owner.MaxValue) - 4;

            oldValue = Math.Max(1.0, oldValue);
            newValue = Math.Max(1.0, newValue);

            var smoothAnimation = new DoubleAnimation(oldValue, newValue, new TimeSpan(0, 0, 0, 0, 150));
            owner.BeginAnimation(Bar.ActualValueProperty, smoothAnimation, HandoffBehavior.Compose);

            var smoothDelayedAnimation = new DoubleAnimation(oldValue, newValue, new TimeSpan(0, 0, 0, 0, 150))
            {
                BeginTime = new TimeSpan(0, 0, 0, 0, 500)
            };
            owner.BeginAnimation(Bar.ActualValueDelayedProperty, smoothDelayedAnimation, HandoffBehavior.Compose);
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            
            double value = (double)e.NewSize.Width * ((double)Value / MaxValue) - (BorderThickness.Left + BorderThickness.Right);

            value = Math.Max(1.0, value);
            
            if (double.IsNaN(value))
            {
                (sender as Bar).InvalidateVisual();
                return;
            }

            var smoothAnimation = new DoubleAnimation(value, value, new TimeSpan(0, 0, 0, 0, 50));

            BeginAnimation(Bar.ActualValueProperty, smoothAnimation, HandoffBehavior.Compose);
            BeginAnimation(Bar.ActualValueDelayedProperty, smoothAnimation, HandoffBehavior.Compose);
        }
    }
}
