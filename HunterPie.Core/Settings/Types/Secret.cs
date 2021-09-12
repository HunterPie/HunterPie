using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace HunterPie.Core.Settings.Types
{
    public class Secret
    {
        public string Value { get; set; }

        public override string ToString()
        {
            return Value;
        }
    }
}
