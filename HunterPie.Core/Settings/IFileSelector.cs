using System.Collections.ObjectModel;

namespace HunterPie.Core.Settings
{
    public interface IFileSelector
    {
        public object Current { get; set; }
        public ObservableCollection<object> Elements { get; }
    }
}
