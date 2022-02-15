using HunterPie.Core.Architecture;
using HunterPie.Core.Client;
using HunterPie.Core.Game.Enums;
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
        private double _flinch;
        private double _maxFlinch;
        private double _sever;
        private double _maxSever;
        private int _breaks;
        private int _maxBreaks;
        private bool _isActive;
        private bool _isPartBroken;
        private bool _isPartSevered;
        private PartType _type;

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
        public double Flinch
        {
            get => _flinch;
            set
            {
                if (value != _flinch)
                    RefreshTimer();

                SetValue(ref _flinch, value);
            }
        }
        public double MaxFlinch { get => _maxFlinch; set { SetValue(ref _maxFlinch, value); } }
        public double MaxTenderize { get => _maxTenderize; set { SetValue(ref _maxTenderize, value); } }
        public double Sever
        {
            get => _sever;
            set
            {
                if (value != _sever)
                    RefreshTimer();

                SetValue(ref _sever, value);
            }
        }
        public double MaxSever { get => _maxSever; set { SetValue(ref _maxSever, value); } }
        public int Breaks { get => _breaks; set { SetValue(ref _breaks, value); } }
        public int MaxBreaks { get => _maxBreaks; set { SetValue(ref _maxBreaks, value); } }
        public bool IsActive { get => _isActive; set { SetValue(ref _isActive, value); } }
        public bool IsPartBroken { get => _isPartBroken; set { SetValue(ref _isPartBroken, value); } }
        public bool IsPartSevered { get => _isPartSevered; set { SetValue(ref _isPartSevered, value); } }
        public PartType Type { get => _type; set { SetValue(ref _type, value); } }

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
            if (!IsActive)
                return;

            IsActive = false;
        }

        private void OnDelayTimeUpdate(object sender, PropertyChangedEventArgs e)
        {
            timeout.Interval = ClientConfig.Config.Overlay.BossesWidget.AutoHidePartsDelay.Current * 1000;
            RefreshTimer();
        }

        private void RefreshTimer()
        {
            IsActive = true;
            timeout.Stop();
            timeout.Start();
        }

        protected virtual void DisposeResources()
        {
            ClientConfig.Config.Overlay.BossesWidget.AutoHidePartsDelay.PropertyChanged -= OnDelayTimeUpdate;
            timeout.Dispose();
        }

        public void Dispose()
        {
            DisposeResources();
        }
    }
}
