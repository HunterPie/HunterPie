using System.Windows.Media;

namespace HunterPie.GUI.Parts.Sidebar.ViewModels
{
    public interface ISideBarElement
    {
        public ImageSource Icon { get; }
        public string Text { get; }
        public bool IsActivable { get; }
        public bool IsEnabled { get; }
        public bool ShouldNotify { get; }

        public void ExecuteOnClick();
    }
}
