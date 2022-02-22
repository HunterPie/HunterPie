using HunterPie.UI.Architecture.Test;
using System;

namespace HunterPie.UI.Overlay.Widgets.Wirebug.ViewModel
{
    public class MockWirebugsViewModel : WirebugsViewModel
    {
        public MockWirebugsViewModel()
        {
            Elements.Add(
                new() 
                {
                    Cooldown = 5,
                    MaxCooldown = 12,
                    OnCooldown = true
                }
            );
            Elements.Add(
                new()
                {
                    Cooldown = 0,
                    MaxCooldown = 12,
                    OnCooldown = false
                }
            );
            Elements.Add(
                new()
                {
                    Cooldown = 5,
                    MaxCooldown = 12,
                    Timer = 200,
                    MaxTimer = 300,
                    OnCooldown = true
                }
            );
            MockBehavior.Run(() =>
            {
                foreach (var vm in Elements)
                {
                    if (!vm.OnCooldown)
                        continue;

                    vm.Cooldown -= 0.01;
                    vm.Timer -= Math.Min(vm.MaxTimer, 0.01);

                    if (vm.Timer <= 0)
                        vm.Timer = vm.MaxTimer;

                    if (vm.Cooldown <= 0)
                    {
                        vm.Cooldown = 0;
                        vm.OnCooldown = false;
                    }
                        
                }
            }, 0.01f);
        }
    }
}
