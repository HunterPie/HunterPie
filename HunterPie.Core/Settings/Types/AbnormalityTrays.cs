using HunterPie.Core.Client.Configuration.Overlay;
using System.Collections.ObjectModel;

namespace HunterPie.Core.Settings.Types
{
    public class AbnormalityTrays
    {
        public ObservableCollection<AbnormalityWidgetConfig> Trays { get; set; } = new();

        public AbnormalityWidgetConfig this[int index] => Trays[index];
    }
}
