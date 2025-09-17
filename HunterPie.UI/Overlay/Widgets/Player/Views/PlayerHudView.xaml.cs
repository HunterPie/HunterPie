using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Enums;
using HunterPie.UI.Architecture.Animation;
using HunterPie.UI.Overlay.Controls;
using HunterPie.UI.Overlay.Widgets.Player.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using ResourcesService = HunterPie.UI.Assets.Application.Resources;

namespace HunterPie.UI.Overlay.Widgets.Player.Views;

/// <summary>
/// Interaction logic for PlayerHudView.xaml
/// </summary>
public partial class PlayerHudView
{
    private readonly BrushAnimation _currentHealthBarAnimation = new BrushAnimation();
    private readonly BrushAnimation _currentStaminaBarAnimation = new BrushAnimation();
    private readonly BrushAnimation _currentHealthRecoverableAnimation = new BrushAnimation();

    private readonly Brush _defaultHealthBrush = ResourcesService.Get<Brush>("Brushes.Widgets.Player.Health.Default");
    private readonly Brush _defaultStaminaBrush = ResourcesService.Get<Brush>("Brushes.Widgets.Player.Stamina.Default");
    private readonly Brush _defaultRecoverableBrush = ResourcesService.Get<Brush>("Brushes.Widgets.Player.Health.Recoverable");

    private readonly AbnormalityCategory[] _healthCategoriesPriority = { AbnormalityCategory.Effluvia, AbnormalityCategory.Poison, AbnormalityCategory.Fire, AbnormalityCategory.Bleed };
    private readonly AbnormalityCategory[] _staminaCategoriesPriority = { AbnormalityCategory.Water, AbnormalityCategory.Ice };
    private readonly AbnormalityCategory[] _recoverableCategoriesPriority = { AbnormalityCategory.NaturalHealing };

    private readonly Dictionary<AbnormalityCategory, Brush> _abnormalityColors = new()
    {
        { AbnormalityCategory.Fire, ResourcesService.Get<Brush>("Brushes.Widgets.Player.Health.Fire") },
        { AbnormalityCategory.Poison, ResourcesService.Get<Brush>("Brushes.Widgets.Player.Health.Poison") },
        { AbnormalityCategory.Bleed, ResourcesService.Get<Brush>("Brushes.Widgets.Player.Health.Bleed") },
        { AbnormalityCategory.Effluvia, ResourcesService.Get<Brush>("Brushes.Widgets.Player.Health.Effluvia") },
        { AbnormalityCategory.Ice, ResourcesService.Get<Brush>("Brushes.Widgets.Player.Stamina.Ice") },
        { AbnormalityCategory.Water, ResourcesService.Get<Brush>("Brushes.Widgets.Player.Stamina.Water") },
        { AbnormalityCategory.NaturalHealing, ResourcesService.Get<Brush>("Brushes.Widgets.Player.Health.NaturalHeal") },
    };

    private PlayerHudViewModel ViewModel => (PlayerHudViewModel)DataContext;

    public PlayerHudView()
    {
        InitializeComponent();
    }

    private void HookEvents()
    {
        ViewModel.ActiveAbnormalities.CollectionChanged += OnActiveAbnormalitiesCollectionChanged;
    }

    private void UnhookEvents()
    {
        ViewModel.ActiveAbnormalities.CollectionChanged -= OnActiveAbnormalitiesCollectionChanged;
    }

    private void OnLoad(object sender, RoutedEventArgs e)
    {
        HookEvents();
    }

    private void OnUnload(object sender, RoutedEventArgs e)
    {
        UnhookEvents();
    }

    private void ResetAnimation(Bar bar, BrushAnimation animation, Brush defaultBrush) =>
        bar.BeginAnimation(Bar.ForegroundProperty, BuildBarAnimation(animation, defaultBrush), HandoffBehavior.SnapshotAndReplace);

    private void AnimateBar(Bar bar, BrushAnimation animation, Brush brush)
    {
        if (animation.To == brush)
            return;

        bar.BeginAnimation(
            Bar.ForegroundProperty,
            BuildBarAnimation(animation, brush),
            HandoffBehavior.SnapshotAndReplace
        );
    }

    private void HandleAbnormalityCategoryChange() =>
        ViewModel.UIThread.Invoke(() =>
        {

            var categories = ViewModel.ActiveAbnormalities.ToHashSet();

            AbnormalityCategory healthAbnormality = categories.FindPriority(_healthCategoriesPriority);
            AbnormalityCategory staminaAbnormality = categories.FindPriority(_staminaCategoriesPriority);
            AbnormalityCategory recoverableAbnormality = categories.FindPriority(_recoverableCategoriesPriority);

            if (healthAbnormality == AbnormalityCategory.None)
                ResetAnimation(PART_HealthBar, _currentHealthBarAnimation, _defaultHealthBrush);
            else
                AnimateBar(PART_HealthBar, _currentHealthBarAnimation, _abnormalityColors[healthAbnormality]);

            if (staminaAbnormality == AbnormalityCategory.None)
                ResetAnimation(PART_StaminaBar, _currentStaminaBarAnimation, _defaultStaminaBrush);
            else
                AnimateBar(PART_StaminaBar, _currentStaminaBarAnimation, _abnormalityColors[staminaAbnormality]);

            if (recoverableAbnormality == AbnormalityCategory.None)
                ResetAnimation(PART_RecoverableHealthBar, _currentHealthRecoverableAnimation, _defaultRecoverableBrush);
            else
                AnimateBar(PART_RecoverableHealthBar, _currentHealthRecoverableAnimation, _abnormalityColors[recoverableAbnormality]);

        });

    private void OnActiveAbnormalitiesCollectionChanged(object sender, NotifyCollectionChangedEventArgs e) => HandleAbnormalityCategoryChange();

    private BrushAnimation BuildBarAnimation(BrushAnimation animation, Brush brush)
    {
        animation.To = brush;
        animation.Duration = new Duration(TimeSpan.FromMilliseconds(500));

        return animation;
    }
}