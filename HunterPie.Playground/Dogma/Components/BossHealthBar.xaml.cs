using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace HunterPie.Playground.Dogma.Components;
/// <summary>
/// Interaction logic for BossHealthBar.xaml
/// </summary>
public partial class BossHealthBar
{
    private readonly DoubleAnimation _bufferAnimation = new()
    {
        EasingFunction = new QuadraticEase(),
        Duration = new Duration(TimeSpan.FromMilliseconds(300))
    };

    private readonly DispatcherTimer _timer;

    public double Current { get => (double)GetValue(CurrentProperty); set => SetValue(CurrentProperty, value); }

    // Using a DependencyProperty as the backing store for Current.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty CurrentProperty =
        DependencyProperty.Register(nameof(Current), typeof(double), typeof(BossHealthBar), new PropertyMetadata(0.0, OnCurrentValueChanged));

    public double Max { get => (double)GetValue(MaxProperty); set => SetValue(MaxProperty, value); }

    // Using a DependencyProperty as the backing store for Max.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty MaxProperty =
        DependencyProperty.Register(nameof(Max), typeof(double), typeof(BossHealthBar), new PropertyMetadata(0.0));

    public double CurrentBuffer { get => (double)GetValue(CurrentBufferProperty); set => SetValue(CurrentBufferProperty, value); }

    // Using a DependencyProperty as the backing store for CurrentBuffer.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty CurrentBufferProperty =
        DependencyProperty.Register(nameof(CurrentBuffer), typeof(double), typeof(BossHealthBar), new PropertyMetadata(0.0));

    public BossHealthBar()
    {
        InitializeComponent();

        _timer = new DispatcherTimer(DispatcherPriority.Render)
        {
            Interval = TimeSpan.FromMilliseconds(500)
        };

        _timer.Tick += OnTimerTick;
    }

    private static void OnCurrentValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not BossHealthBar component)
            return;

        if (component.Max <= 0.0)
            return;

        if (double.IsNaN(component.Current) || double.IsInfinity(component.Current))
            return;

        if (component.Current > component.CurrentBuffer)
        {
            component.BeginAnimation(
                dp: CurrentBufferProperty,
                animation: new DoubleAnimation() { From = component.Current, To = component.Current },
                handoffBehavior: HandoffBehavior.SnapshotAndReplace
            );
            return;
        }

        component._bufferAnimation.From = component.CurrentBuffer;
        component._bufferAnimation.To = component.Current;

        component._timer.Stop();
        component._timer.Start();
    }

    private void OnTimerTick(object? _, EventArgs __)
    {
        BeginAnimation(
            dp: CurrentBufferProperty,
            animation: _bufferAnimation,
            handoffBehavior: HandoffBehavior.SnapshotAndReplace
        );

        _timer.Stop();
    }
}