using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.Core.Game.Entity.Enemy;
using HunterPie.Core.Game.Enums;
using HunterPie.Core.Observability.Logging;
using HunterPie.UI.Architecture;
using System.Collections.ObjectModel;

namespace HunterPie.UI.Overlay.Widgets.Monster.ViewModels;

public class BossMonsterViewModel(MonsterWidgetConfig config) : ViewModel
{
    private readonly ILogger _logger = LoggerFactory.Create();

    public MonsterWidgetConfig Config { get; } = config;
    private readonly ObservableCollection<MonsterPartViewModel> _parts = new();
    private readonly ObservableCollection<MonsterAilmentViewModel> _ailments = new();
    private readonly ObservableCollection<Element> _weaknesses = new();
    private readonly ObservableCollection<string> _types = new();

    // Monster data
    public string? Name
    {
        get;
        set => SetValue(ref field, value);
    }

    public double Health
    {
        get;
        set
        {
            SetValue(ref field, value);
            HealthPercentage = Health / MaxHealth * 100;
        }
    }
    public double MaxHealth
    {
        get;
        set => SetValue(ref field, value);
    }

    public double HealthPercentage
    {
        get;
        private set => SetValue(ref field, value);
    }

    public double Stamina
    {
        get;
        set => SetValue(ref field, value);
    }

    public double MaxStamina
    {
        get;
        set => SetValue(ref field, value);
    }

    public Crown Crown
    {
        get;
        set => SetValue(ref field, value);
    }

    public Target TargetType
    {
        get;
        set => SetValue(ref field, value);
    } = Target.None;

    public double CaptureThreshold
    {
        get;
        set => SetValue(ref field, value);
    }
    public bool IsCapturable
    {
        get;
        set => SetValue(ref field, value);
    }

    public bool CanBeCaptured
    {
        get;
        set => SetValue(ref field, value);
    }

    public bool IsQurio { get; set; }

    public VariantType Variant
    {
        get;
        set => SetValue(ref field, value);
    }

    public ref readonly ObservableCollection<MonsterPartViewModel> Parts => ref _parts;
    public ref readonly ObservableCollection<MonsterAilmentViewModel> Ailments => ref _ailments;
    public ref readonly ObservableCollection<Element> Weaknesses => ref _weaknesses;
    public ref readonly ObservableCollection<string> Types => ref _types;

    // Monster states
    public bool IsEnraged
    {
        get;
        set => SetValue(ref field, value);
    }

    public bool IsTarget
    {
        get;
        set => SetValue(ref field, value);
    }

    public bool IsLoadingIcon
    {
        get;
        set => SetValue(ref field, value);
    } = true;

    public string? Icon
    {
        get;
        set => SetValue(ref field, value);
    }

    public bool IsAlive { get; set => SetValue(ref field, value); }

}