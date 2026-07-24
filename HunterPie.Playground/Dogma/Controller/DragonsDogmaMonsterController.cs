using HunterPie.Core.Game.Entity.Enemy;
using HunterPie.Playground.Dogma.ViewModels;
using System.Windows.Threading;

namespace HunterPie.Playground.Dogma.Controller;

internal class DragonsDogmaMonsterController(
    Dispatcher dispatcher,
    IMonster context,
    DragonsDogmaMonsterViewModel viewModel
) : IDisposable
{
    public DragonsDogmaMonsterViewModel ViewModel => viewModel;

    public void Initialize()
    {
        context.OnHealthChange += OnHealthChange;

        Load();
    }

    public void Dispose()
    {
        context.OnHealthChange -= OnHealthChange;
    }

    private void Load()
    {
        viewModel.Name = context.Name;

        UpdateHealth(
            current: context.Health,
            max: context.MaxHealth
        );
    }

    private void OnHealthChange(object? sender, EventArgs e) =>
        UpdateHealth(
            current: context.Health,
            max: context.MaxHealth
        );

    private void UpdateHealth(double current, double max) => dispatcher.BeginInvoke(() =>
    {
        int sections = (int)Math.Ceiling(max / 5000);
        double healthPerSection = max / sections;
        double currentSection = Math.Ceiling(current / healthPerSection);
        double currentSectionStart = Math.Max(0, currentSection - 1) * healthPerSection;
        double normalizedHealth = Math.Max(current - currentSectionStart, 0);

        viewModel.MaxHealth = healthPerSection;
        viewModel.Health = normalizedHealth;
        viewModel.MaxSections = sections;
        viewModel.Section = (int)currentSection;
    });

}