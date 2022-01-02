using HunterPie.Core.Architecture;
using HunterPie.Core.Game.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HunterPie.UI.Overlay.Widgets.Damage.ViewModel
{
    internal class MockMeterViewModel : Bindable
    {
        public ObservableCollection<PlayerViewModel> Players { get; } = new()
        {
            new()
            {
                Name = "Player 1",
                Weapon = Weapon.Bow,
                Damage = 15000,
                DPS = 50.37,
                Percentage = 70.36,
                IsIncreasing = true,
                IsUser = false
            },
            new()
            {
                Name = "Player 2",
                Weapon = Weapon.ChargeBlade,
                Damage = 15000,
                DPS = 50.37,
                Percentage = 70.36,
                IsIncreasing = true,
                IsUser = true
            },
            new()
            {
                Name = "Player 3",
                Weapon = Weapon.Greatsword,
                Damage = 15000,
                DPS = 50.37,
                Percentage = 70.36,
                IsIncreasing = false,
                IsUser = false
            },
            new()
            {
                Name = "Player 4",
                Weapon = Weapon.HuntingHorn,
                Damage = 15000,
                DPS = 50.37,
                Percentage = 70.36,
                IsIncreasing = true,
                IsUser = false
            },
        };
    }
}
