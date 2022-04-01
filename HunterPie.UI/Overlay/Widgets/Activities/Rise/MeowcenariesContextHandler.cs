using HunterPie.Core.Game.Rise;
using HunterPie.UI.Overlay.Widgets.Activities.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HunterPie.UI.Overlay.Widgets.Activities.Rise
{
    internal class MeowcenariesContextHandler : IContextHandler
    {
        private readonly MHRContext _context;
        private readonly MeowcenariesViewModel ViewModel = new();

        public MeowcenariesContextHandler(MHRContext context)
        {
            _context = context;
        }

        public void HookEvents()
        {

        }

        public void UnhookEvents()
        {
            throw new NotImplementedException();
        }
    }
}
