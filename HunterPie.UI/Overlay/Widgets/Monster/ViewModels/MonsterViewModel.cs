using HunterPie.Core.Architecture;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HunterPie.UI.Overlay.Widgets.Monster.ViewModels
{
    public class MonsterViewModel : Notifiable
    {
        private string name;
        private string em;
        private double health;
        private double maxHealth;
        private double healthPercentage;
        private double stamina;
        private double maxStamina;
        private bool isEnraged;

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
        
        public readonly ObservableCollection<MonsterPartViewModel> Parts = new();
        public readonly ObservableCollection<MonsterAilmentViewModel> Ailments = new();

        // Monster states
        public bool IsEnraged
        {
            get => isEnraged;
            set { SetValue(ref isEnraged, value); }
        }
        public bool IsTarget { get; set; }
    }
}
