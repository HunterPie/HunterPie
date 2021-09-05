using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HunterPie.UI.Architecture
{
    public interface IView<TViewModel>
    {
        public TViewModel Model { get; }
    }
}
