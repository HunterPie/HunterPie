using HunterPie.UI.Architecture;

namespace HunterPie.GUI.ViewModels
{
    internal class MainViewModel : ViewModel
    {
        private bool _isNotificationsOpen;

        public bool IsNotificationsOpen { get => _isNotificationsOpen; set { SetValue(ref _isNotificationsOpen, value); } }
    }
}
