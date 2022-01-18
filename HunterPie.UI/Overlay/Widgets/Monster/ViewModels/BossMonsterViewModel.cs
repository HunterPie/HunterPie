using HunterPie.Core.Architecture;
using HunterPie.Core.Game.Enums;
using System.Collections.ObjectModel;

namespace HunterPie.UI.Overlay.Widgets.Monster.ViewModels
{
    public class BossMonsterViewModel : Bindable
    {
        private string name;
        private string em;
        private double health;
        private double maxHealth;
        private double healthPercentage;
        private double stamina;
        private double maxStamina;
        private bool isTarget;
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
            set
            {
                SetValue(ref health, value);
                HealthPercentage = Health / MaxHealth * 100;
            }
        }
        public double MaxHealth
        {
            get => maxHealth;
            set { SetValue(ref maxHealth, value); }
        }
        
        public double HealthPercentage
        {
            get => healthPercentage;
            private set { SetValue(ref healthPercentage, value); }
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

        public bool IsTarget
        {
            get => isTarget;
            set { SetValue(ref isTarget, value); }
        }
    }
}
