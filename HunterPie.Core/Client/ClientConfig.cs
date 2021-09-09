using HunterPie.Core.Client.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HunterPie.Core.Client
{
    public class ClientConfig
    {
        private readonly static Config _config = new Config();
        public static Config Config
        {
            get => _config;
        }

    }
}
