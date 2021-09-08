using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HunterPie.Core.Settings
{
    public interface IFileSelector
    {
        public object Current { get; set; }
        public object[] List();
    }
}
