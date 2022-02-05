using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HunterPie.UI.Controls.Settings.Custom.Abnormality
{
    public class AbnormalityCollectionViewModel
    {
        public string Name { get; set; }
        public string Icon { get; set; }
        public ObservableCollection<AbnormalityViewModel> Abnormalities { get; } = new();
    }
}
