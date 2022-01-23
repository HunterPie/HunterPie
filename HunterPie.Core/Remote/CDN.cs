using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HunterPie.Core.Remote
{
    public class CDN
    {
        const string CDN_BASE_URL = "https://cdn.hunterpie.com";

        public static string GetMonsterIcon(string imageName)
        {
            return "";
        }

        public static string GetMonsterIconUrl(string imagename)
        {
            return $"{CDN_BASE_URL}/Monsters/Icons/{imagename}.png";
        }
    }
}
