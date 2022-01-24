using HunterPie.Core.Architecture;
using HunterPie.Core.Client;
using HunterPie.Core.Game.Enums;
using HunterPie.Core.Remote;
using System.Collections.ObjectModel;
using System.IO;

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
        private Target _targetType = Target.None;
        private bool isEnraged;
        private Crown _crown;
        private string _icon;
        private bool _isLoadingIcon = true;
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

        public Target TargetType
        {
            get => _targetType;
            set { SetValue(ref _targetType, value); }
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

        public bool IsLoadingIcon
        {
            get => _isLoadingIcon;
            set { SetValue(ref _isLoadingIcon, value); }
        }

        public string Icon
        {
            get => _icon;
            set { SetValue(ref _icon, value); }
        }

        public async void FetchMonsterIcon()
        {
            IsLoadingIcon = true;
            string imageName = BuildIconName();
            string imagePath = ClientInfo.GetPathFor($"Assets/Monsters/Icons/{em}.png");

            // If file doesn't exist locally, we can check for the CDN
            if (!File.Exists(imagePath))
                imagePath = await CDN.GetMonsterIconUrl(imageName);

            IsLoadingIcon = false;
            Icon = imagePath;
        }

        private string BuildIconName()
        {
            string monsterEm = Em;
            bool isRise = Em.StartsWith("Rise");

            if (!isRise)
                monsterEm += "_ID";

            return monsterEm;
        }
    }
}
