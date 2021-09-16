using HunterPie.Core.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HunterPie.UI.Overlay
{
    public interface IWidget
    {
        public IWidgetSettings Settings { get; }
        public string Title { get; }
    }
}
