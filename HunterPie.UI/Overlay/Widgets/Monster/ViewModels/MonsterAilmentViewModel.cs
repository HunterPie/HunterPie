using HunterPie.Core.Architecture;
using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration.Overlay;
using System;
using System.ComponentModel;
using System.Timers;

namespace HunterPie.UI.Overlay.Widgets.Monster.ViewModels
{
    public class MonsterAilmentViewModel : Bindable, IDisposable
    {
        // Internal logic
        private readonly Timer timeout = new(15000);
        private readonly MonsterWidgetConfig _config;

        private string _name;
        private double _timer;
        private double _maxTimer;
        private double _buildup;
        private double _maxBuildup;
        private int _count;
        private bool _isActive;

        private object sync = new();

        public string Name { get => _name; set { SetValue(ref _name, value); } }
        public double Timer
        {
            get => _timer;
            set
            {
                if (value != _timer)
                    RefreshTimer();

                SetValue(ref _timer, value);
            }
        }
        public double MaxTimer { get => _maxTimer; set { SetValue(ref _maxTimer, value); } }
        public double Buildup
        {
            get => _buildup;
            set
            {
                if (value != _buildup)
                    RefreshTimer();

                SetValue(ref _buildup, value);
            }
        }
        public double MaxBuildup { get => _maxBuildup; set { SetValue(ref _maxBuildup, value); } }
        public int Count { get => _count; set { SetValue(ref _count, value); } }
        public bool IsActive { get => _isActive; set { SetValue(ref _isActive, value); } }

        public MonsterAilmentViewModel(MonsterWidgetConfig config)
        {
            _config = config;
            timeout = new(config.AutoHideAilmentsDelay.Current * 1000)
            {
                AutoReset = true
            };
            timeout.Elapsed += OnHideTimerTick;
            timeout.Start();

            config.AutoHideAilmentsDelay.PropertyChanged += OnDelayTimeUpdate;
        }

        private void UpdateActiveState(object sender, ElapsedEventArgs e) => IsActive = false;

        private void OnHideTimerTick(object sender, ElapsedEventArgs e)
        {
            lock (sync)
            {
                if (!IsActive)
                    return;

                IsActive = false;
            }
        }

        private void OnDelayTimeUpdate(object sender, PropertyChangedEventArgs e)
        {
            timeout.Interval = _config.AutoHideAilmentsDelay.Current * 1000;
            RefreshTimer();
        }

        private void RefreshTimer()
        {
            lock (sync)
            {
                IsActive = true;
                timeout.Stop();
                timeout.Start();
            }
        }

        protected virtual void DisposeResources()
        {
            _config.AutoHideAilmentsDelay.PropertyChanged -= OnDelayTimeUpdate;
            timeout.Dispose();
        }

        public void Dispose()
        {
            DisposeResources();
        }
    }
}
