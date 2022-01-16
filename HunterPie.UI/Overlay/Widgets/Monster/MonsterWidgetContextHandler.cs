using HunterPie.Core.Game;
using HunterPie.Core.Game.Environment;
using HunterPie.UI.Overlay.Widgets.Monster.ViewModels;
using HunterPie.UI.Overlay.Widgets.Monster.Views;
using System.Linq;
using System.Windows;

namespace HunterPie.UI.Overlay.Widgets.Monster
{
    public class MonsterWidgetContextHandler
    {
        private readonly MonstersViewModel ViewModel;
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
            foreach (IMonster monster in Context.Game.Monsters)
                ViewModel.Monsters.Add(new MonsterContextHandler(monster));
        }

        private void HookEvents()
        {
            Context.Game.OnMonsterSpawn += OnMonsterSpawn;
            Context.Game.OnMonsterDespawn += OnMonsterDespawn;
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
        }

        private void OnMonsterSpawn(object sender, IMonster e)
        {
            Application.Current.Dispatcher.Invoke(() => ViewModel.Monsters.Add(new MonsterContextHandler(e)));
        }
    }
}
