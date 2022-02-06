using System.Collections.ObjectModel;

namespace HunterPie.UI.Controls.Settings.Custom.Abnormality
{
    public class AbnormalityCollectionViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public object Icon { get; set; }
        public ObservableCollection<AbnormalityViewModel> Abnormalities { get; } = new();
    }
}
