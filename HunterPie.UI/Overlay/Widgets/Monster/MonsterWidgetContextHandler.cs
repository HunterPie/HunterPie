using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration;
using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.Core.Domain.Enums;
using HunterPie.Core.Game;
using HunterPie.Core.Game.Environment;
using HunterPie.Core.System;
using HunterPie.UI.Overlay.Widgets.Monster.ViewModels;
using HunterPie.UI.Overlay.Widgets.Monster.Views;
using System;
using System.Linq;
using System.Windows;

namespace HunterPie.UI.Overlay.Widgets.Monster
{
    public class MonsterWidgetContextHandler : IContextHandler
    {
        private readonly MonstersViewModel ViewModel;
        private readonly MonstersView View;
        private MonsterWidgetConfig Settings => View.Settings;
        private readonly Context Context;

        public MonsterWidgetContextHandler(Context context)
        {
            OverlayConfig config = ClientConfigHelper.GetOverlayConfigFrom(ProcessManager.Game);

            View = new MonstersView(config.BossesWidget);
            WidgetManager.Register<MonstersView, MonsterWidgetConfig>(View);

            ViewModel = View.ViewModel;
            Context = context;

            UpdateData();
            HookEvents();
        }

        private void UpdateData()
        {
            foreach (IMonster monster in Context.Game.Monsters)
            {
                monster.OnTargetChange += OnTargetChange;
                ViewModel.Monsters.Add(new MonsterContextHandler(monster, Settings));
            }
            CalculateVisibleMonsters();
        }

        public void HookEvents()
        {
            Context.Game.OnMonsterSpawn += OnMonsterSpawn;
            Context.Game.OnMonsterDespawn += OnMonsterDespawn;
        }

        public void UnhookEvents()
        {
            Context.Game.OnMonsterSpawn -= OnMonsterSpawn;
            Context.Game.OnMonsterDespawn -= OnMonsterDespawn;
            WidgetManager.Unregister<MonstersView, MonsterWidgetConfig>(View);
        }

        private void OnMonsterDespawn(object sender, IMonster e)
        {
            Application.Current.Dispatcher.Invoke(() => {
                MonsterContextHandler monster = ViewModel.Monsters
                    .Cast<MonsterContextHandler>()
                    .FirstOrDefault(handler => handler.Context == e);
                    
                if (monster is null)
                    return;

                ViewModel.Monsters.Remove(monster);
            });

            e.OnTargetChange -= OnTargetChange;
            CalculateVisibleMonsters();
        }

        private void OnMonsterSpawn(object sender, IMonster e)
        {
            Application.Current.Dispatcher.Invoke(() => ViewModel.Monsters.Add(new MonsterContextHandler(e, Settings)));
            
            e.OnTargetChange += OnTargetChange;
            CalculateVisibleMonsters();
        }

        private void OnTargetChange(object sender, EventArgs e) => CalculateVisibleMonsters();

        private void CalculateVisibleMonsters()
        {
            int targets = Context.Game.Monsters.Count(m => m.IsTarget);

            ViewModel.VisibleMonsters = Settings.ShowOnlyTarget.Value switch
            {
                true => targets,
                false => targets == 0 ? Context.Game.Monsters.Count : targets,
            };
            ViewModel.MonstersCount = Context.Game.Monsters.Count;
        }
    }
}
