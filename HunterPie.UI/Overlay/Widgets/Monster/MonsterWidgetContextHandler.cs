using HunterPie.Core.Game;
using HunterPie.Core.Game.Environment;
using HunterPie.UI.Overlay.Widgets.Monster.ViewModels;
using HunterPie.UI.Overlay.Widgets.Monster.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

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

            HookEvents();
        }

        private void HookEvents()
        {
            Context.Game.OnMonsterSpawn += OnMonsterSpawn;
        }

        private void OnMonsterSpawn(object sender, IMonster e)
        {
            Application.Current.Dispatcher.Invoke(() => ViewModel.Monsters.Add(new MonsterContextHandler(e)));
        }
    }
}
