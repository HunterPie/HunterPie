using HunterPie.Core.Domain.Process;
using System.Collections.Generic;

namespace HunterPie.Core.Game.Rise.Data
{
    public class MHRStrings
    {

        private readonly IProcessManager _process;
        private Dictionary<int, string> _mockMonsterNames = new()
        {
            { 0, "Rathian" },
            { 1, "Apex Rathian" },
            { 2, "Rathalos" },
            { 3, "Apex Rathalos" },
            { 4, "Khezu" },
            { 5, "Basarios" },
            { 6, "Diablos" },
            { 7, "Apex Diablos" },
            { 8, "Rajang" },
            { 9, "Kushala Daora" },
            { 10, "Chameleos" },
            { 11, "Teostra" },
            { 12, "Tigrex" },
            { 13, "Nargacuga" },
            { 14, "Barioth" },
            { 15, "Barroth" },
            { 16, "Royal Ludroth" },
            { 17, "Great Baggi" },
            { 18, "Zinogre" },
            { 19, "Apex Zinogre" },
            { 20, "Great Wroggi" },
            { 21, "Arzuros" },
            { 22, "Apex Arzuros" },
            { 23, "Lagombi" },
            { 24, "Volvidon" },
            { 25, "Mizutsune" },
            { 26, "Apex Mizutsune" },
            { 27, "Crimson Glow Valstrax" },
            { 28, "Magnamalo" },
            { 29, "Bishaten" },
            { 30, "Aknosom" },
            { 31, "Tetranadon" },
            { 32, "Somnacanth" },
            { 33, "Rakna-Kadaki" },
            { 34, "Almudron" },
            { 35, "Wind Serpent Ibushi" },
            { 36, "Goss Harag" },
            { 37, "Great Izuchi" },
            { 38, "Thunder Serpent Narwa" },
            { 39, "Narwa the Allmother" },
            { 40, "Anjanath" },
            { 41, "Pukei-Pukei" },
            { 42, "Kulu-Ya-Ku" },
            { 43, "Jyuratodus" },
            { 44, "Tobi-Kadachi" },
            { 45, "Bazelgeuse" },
            { 46, "Kelbi" },
            { 47, "Felyne" },
            { 48, "Melynx" },
            { 49, "Bullfango" },
            { 50, "Popo" },
            { 51, "Anteka" },
            { 52, "Remobra" },
            { 53, "Rhenoplos" },
            { 54, "Bnahabra" },
            { 55, "Altaroth" },
            { 56, "Gajau" },
            { 57, "Jaggi" },
            { 58, "Jaggia" },
            { 59, "Baggi" },
            { 60, "Delex" },
            { 61, "Ludroth" },
            { 62, "Uroktor" },
            { 63, "Slagtoth" },
            { 64, "Gargwa" },
            { 65, "Wroggi" },
            { 66, "Zamite" },
            { 67, "Jagras" },
            { 68, "Kestodon" },
            { 69, "Bombadgy" },
            { 70, "Izuchi" },
            { 71, "Rachnoid" }
        };

        public MHRStrings(IProcessManager process)
        {
            _process = process;
        }

        public string GetMonsterNameById(int id)
        {
            if (_mockMonsterNames.ContainsKey(id))
                return _mockMonsterNames[id];

            return "Unknown";
        }
    }
}
