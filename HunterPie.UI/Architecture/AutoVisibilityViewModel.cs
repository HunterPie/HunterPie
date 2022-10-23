using System;
using System.ComponentModel;
using System.Timers;
using Range = HunterPie.Core.Settings.Types.Range;

namespace HunterPie.UI.Architecture;

public class AutoVisibilityViewModel : ViewModel, IDisposable
{
    private const int MILLISECOND = 1000;
    private readonly Range Timeout;
    private Timer _timer;

    private bool _isActive;

    public bool IsActive { get => _isActive; set => SetValue(ref _isActive, value); }

    public AutoVisibilityViewModel(Range timeout)
    {
        Timeout = timeout;
        _timer = new(Timeout.Current * MILLISECOND)
        {
            AutoReset = true,
        };

        SetupHooks();

        _timer.Start();
    }

    private void SetupHooks()
    {
        _timer.Elapsed += OnTimerElapsed;
        Timeout.PropertyChanged += OnTimeoutValueChanged;
    }

    private void OnTimerElapsed(object sender, ElapsedEventArgs e)
    {
        if (!IsActive)
            return;

        IsActive = false;
    }

    protected void RefreshTimer()
    {
        if (_timer is null)
            return;

        IsActive = true;
        _timer.Stop();
        _timer.Start();
    }

    private void OnTimeoutValueChanged(object sender, PropertyChangedEventArgs e)
    {
        _timer.Interval = Timeout.Current * MILLISECOND;
        RefreshTimer();
    }

    public virtual void Dispose()
    {
        _timer.Elapsed -= OnTimerElapsed;
        _timer.Dispose();
        _timer = null;
        Timeout.PropertyChanged -= OnTimeoutValueChanged;
        GC.SuppressFinalize(this);
    }
}
