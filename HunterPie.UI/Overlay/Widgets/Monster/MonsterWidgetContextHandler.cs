using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.Core.Game;
using HunterPie.Core.Game.Environment;
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
        private MonsterWidgetConfig Settings => ClientConfig.Config.Overlay.BossesWidget;
        private readonly Context Context;

        public MonsterWidgetContextHandler(Context context)
        {
            var widget = new MonstersView();
            WidgetManager.Register(widget);

            ViewModel = widget.DataContext as MonstersViewModel;
            Context = context;

            UpdateData();
            HookEvents();
        }

        private void UpdateData()
        {
            ViewModel.VisibleMonsters = 0;
            foreach (IMonster monster in Context.Game.Monsters)
            {
                ViewModel.VisibleMonsters += monster.IsTarget || !Settings.ShowOnlyTarget ? 1 : 0;
                monster.OnTargetChange += OnTargetChange;
                ViewModel.Monsters.Add(new MonsterContextHandler(monster));
            }
        }

        private void HookEvents()
        {
            Context.Game.OnMonsterSpawn += OnMonsterSpawn;
            Context.Game.OnMonsterDespawn += OnMonsterDespawn;
        }

        public void UnhookEvents()
        {
            Context.Game.OnMonsterSpawn -= OnMonsterSpawn;
            Context.Game.OnMonsterDespawn -= OnMonsterDespawn;
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
            ViewModel.MonstersCount = Context.Game.Monsters.Count;
        }

        private void OnMonsterSpawn(object sender, IMonster e)
        {
            Application.Current.Dispatcher.Invoke(() => ViewModel.Monsters.Add(new MonsterContextHandler(e)));
            
            e.OnTargetChange += OnTargetChange;
            ViewModel.MonstersCount = Context.Game.Monsters.Count;
        }

        private void OnTargetChange(object sender, EventArgs e)
        {
            int targets = Context.Game.Monsters.Where(m => m.IsTarget).Count();

            ViewModel.VisibleMonsters = Settings.ShowOnlyTarget.Value switch
            {
                true => targets,
                false => targets == 0 ? 3 : targets,
            };
        }

    }
}
