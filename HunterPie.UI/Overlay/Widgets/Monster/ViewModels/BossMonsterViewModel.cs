﻿using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.Core.Game.Enums;
using HunterPie.Core.Observability.Logging;
using HunterPie.Core.Remote;
using HunterPie.UI.Architecture;
using HunterPie.UI.Architecture.Images;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;

namespace HunterPie.UI.Overlay.Widgets.Monster.ViewModels;

public class BossMonsterViewModel : ViewModel
{
    private readonly ILogger _logger = LoggerFactory.Create();

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
    private bool _isAlive;
    private double _captureThreshold;
    private bool _isCapturable;
    private bool _canBeCaptured;

    public MonsterWidgetConfig Config { get; }
    private readonly ObservableCollection<MonsterPartViewModel> _parts = new();
    private readonly ObservableCollection<MonsterAilmentViewModel> _ailments = new();
    private readonly ObservableCollection<Element> _weaknesses = new();
    private readonly ObservableCollection<string> _types = new();

    public BossMonsterViewModel(MonsterWidgetConfig config)
    {
        Config = config;
    }

    // Monster data
    public string Name
    {
        get => name;
        set => SetValue(ref name, value);
    }

    public string Em
    {
        get => em;
        set => SetValue(ref em, value);
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
        set => SetValue(ref maxHealth, value);
    }

    public double HealthPercentage
    {
        get => healthPercentage;
        private set => SetValue(ref healthPercentage, value);
    }

    public double Stamina
    {
        get => stamina;
        set => SetValue(ref stamina, value);
    }

    public double MaxStamina
    {
        get => maxStamina;
        set => SetValue(ref maxStamina, value);
    }

    public Crown Crown
    {
        get => _crown;
        set => SetValue(ref _crown, value);
    }

    public Target TargetType
    {
        get => _targetType;
        set => SetValue(ref _targetType, value);
    }

    public double CaptureThreshold
    {
        get => _captureThreshold;
        set => SetValue(ref _captureThreshold, value);
    }
    public bool IsCapturable
    {
        get => _isCapturable;
        set => SetValue(ref _isCapturable, value);
    }

    public bool CanBeCaptured
    {
        get => _canBeCaptured;
        set => SetValue(ref _canBeCaptured, value);
    }

    public bool IsQurio { get; set; }

    public ref readonly ObservableCollection<MonsterPartViewModel> Parts => ref _parts;
    public ref readonly ObservableCollection<MonsterAilmentViewModel> Ailments => ref _ailments;
    public ref readonly ObservableCollection<Element> Weaknesses => ref _weaknesses;
    public ref readonly ObservableCollection<string> Types => ref _types;

    // Monster states
    public bool IsEnraged
    {
        get => isEnraged;
        set => SetValue(ref isEnraged, value);
    }

    public bool IsTarget
    {
        get => isTarget;
        set => SetValue(ref isTarget, value);
    }

    public bool IsLoadingIcon
    {
        get => _isLoadingIcon;
        set => SetValue(ref _isLoadingIcon, value);
    }

    public string Icon
    {
        get => _icon;
        set => SetValue(ref _icon, value);
    }

    public bool IsAlive { get => _isAlive; set => SetValue(ref _isAlive, value); }

    public async void FetchMonsterIcon()
    {
        IsLoadingIcon = true;

        string imageName = Em;
        string imagePath = ClientInfo.GetPathFor($"Assets/Monsters/Icons/{imageName}.png");

        // If file doesn't exist locally, we can check for the CDN
        if (!File.Exists(imagePath))
            imagePath = await CDN.GetMonsterIconUrl(imageName);

        if (IsQurio)
            imagePath = await FetchQurioIcon(imagePath);

        IsLoadingIcon = false;
        Icon = imagePath;
    }

    private async Task<string> FetchQurioIcon(string defaultImagePath)
    {
        string maskPath = ClientInfo.GetPathFor($"Assets/Monsters/Masks/qurio_mask.png");
        string imageName = $"{Em}_qurio";
        string imagePath = ClientInfo.GetPathFor($"Assets/Monsters/Icons/{imageName}.png");

        if (File.Exists(imagePath))
            return imagePath;

        try
        {
            imagePath = await ImageMergerService.MergeAsync(
                imagePath,
                defaultImagePath,
                maskPath
            );
        }
        catch (Exception ex)
        {
            _logger.Warning($"Failed to generate Qurio icon, defaulting to non-qurio icon. Error: {ex}");
        }

        return imagePath;
    }
}