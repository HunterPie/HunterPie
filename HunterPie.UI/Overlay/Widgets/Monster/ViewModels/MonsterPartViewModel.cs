using HunterPie.Core.Architecture;
using HunterPie.Core.Client;
using System;
using System.ComponentModel;
using System.Timers;

namespace HunterPie.UI.Overlay.Widgets.Monster.ViewModels
{
    public class MonsterPartViewModel : Bindable, IDisposable
    {
        private readonly Timer timeout;

        private string _name;
        private double _health;
        private double _maxHealth;
        private double _tenderize;
        private double _maxTenderize;
        private int _breaks;
        private int _maxBreaks;
        private bool _isActive;

        public string Name { get => _name; set { SetValue(ref _name, value); } }
        public double Health
        {
            get => _health;
            set
            {
                if (value != _health)
                    RefreshTimer();

                SetValue(ref _health, value);
            }
        }
        public double MaxHealth { get => _maxHealth; set { SetValue(ref _maxHealth, value); } }
        public double Tenderize
        {
            get => _tenderize;
            set
            {
                if (value != _tenderize)
                    RefreshTimer();

                SetValue(ref _tenderize, value);
            }
        }
        public double MaxTenderize { get => _maxTenderize; set { SetValue(ref _maxTenderize, value); } }
        public int Breaks { get => _breaks; set { SetValue(ref _breaks, value); } }
        public int MaxBreaks { get => _maxBreaks; set { SetValue(ref _maxBreaks, value); } }
        public bool IsActive { get => _isActive; set { SetValue(ref _isActive, value); } }

        private object sync = new();

        public MonsterPartViewModel()
        {
            timeout = new(ClientConfig.Config.Overlay.BossesWidget.AutoHidePartsDelay.Current * 1000)
            {
                AutoReset = true
            };
            timeout.Elapsed += OnHideTimerTick;
            timeout.Start();

            ClientConfig.Config.Overlay.BossesWidget.AutoHidePartsDelay.PropertyChanged += OnDelayTimeUpdate;
        }

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
            timeout.Interval = ClientConfig.Config.Overlay.BossesWidget.AutoHidePartsDelay.Current * 1000;
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
            ClientConfig.Config.Overlay.BossesWidget.AutoHidePartsDelay.PropertyChanged -= OnDelayTimeUpdate;
            timeout.Dispose();
        }

        public void Dispose() => DisposeResources();
    }
}
