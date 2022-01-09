using HunterPie.Core.Architecture;
using HunterPie.Core.Game.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HunterPie.UI.Overlay.Widgets.Monster.ViewModels
{
    internal class MockMonsterViewModel : Bindable
    {
        private string name;
        private string em;
        private double health;
        private double maxHealth;
        private double healthPercentage;
        private double stamina;
        private double maxStamina;
        private bool isEnraged;
        private Crown _crown;
        private readonly ObservableCollection<MonsterPartViewModel> parts = new();
        private readonly ObservableCollection<MonsterAilmentViewModel> ailments = new();

        // Monster data
        public string Name
        {
            get => name;
            set { SetValue(ref name, value); }
        }
        public string Em
        {
            get => em;
            set { SetValue(ref em, value); }
        }

        public double Health
        {
            get => health;
            set { SetValue(ref health, value); }
        }
        public double MaxHealth
        {
            get => maxHealth;
            set { SetValue(ref maxHealth, value); }
        }
        public double HealthPercentage
        {
            get => healthPercentage;
            set { SetValue(ref healthPercentage, value); }
        }
        public double Stamina
        {
            get => stamina;
            set { SetValue(ref stamina, value); }
        }
        public double MaxStamina
        {
            get => maxStamina;
            set { SetValue(ref maxStamina, value); }
        }
        public Crown Crown
        {
            get => _crown;
            set { SetValue(ref _crown, value); }
        }

        public ref readonly ObservableCollection<MonsterPartViewModel> Parts => ref parts;
        public ref readonly ObservableCollection<MonsterAilmentViewModel> Ailments => ref ailments;

        // Monster states
        public bool IsEnraged
        {
            get => isEnraged;
            set { SetValue(ref isEnraged, value); }
        }
        public bool IsTarget { get; set; }

        public MockMonsterViewModel()
        {
            MockParts();
            MockAilments();
        }

        private void MockParts()
        {
            for (int i = 0; i < 12; i++)
            {
                parts.Add(
                    new MonsterPartViewModel()
                    {
                        Name = $"Part {i}",
                        Health = 200.0,
                        MaxHealth = 250.0,
                        Tenderize = 10.0,
                        MaxTenderize = 10.0,
                        Break = 0,
                        MaxBreaks = 0
                    }
                );
            }
        }

        private void MockAilments()
        {
            for (int i = 0; i < 19; i++)
            {
                ailments.Add(
                    new MonsterAilmentViewModel()
                    {
                        Name = "Ailment",
                        Timer = 100.0,
                        MaxTimer = 100.0,
                        Buildup = 100.0,
                        MaxBuildup = 100.0
                    }
                );
            }
        }
    }
}
