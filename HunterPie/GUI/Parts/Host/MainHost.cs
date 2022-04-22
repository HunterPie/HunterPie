using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace HunterPie.GUI.Parts.Host
{
    internal class MainHost : INotifyPropertyChanged
    {
        private static MainHost _instance;
        public static MainHost Instance
        {
            get
            {
                if (_instance is null)
                    _instance = new();

                return _instance;
            }
        }

        private UIElement _element;

        public event PropertyChangedEventHandler PropertyChanged;
    
        private void N([CallerMemberName]string propertyName = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        
        private MainHost() { }

        public UIElement Element
        {
            get => _element;
            private set
            {
                if (value != _element)
                {
                    _element = value;
                    N();
                }
            } 
        }

        public static void SetMain<T>(T element, bool forceRefresh = false) where T : UIElement
        {
            if (!forceRefresh && IsInstanceOf<T>())
                return;

            Instance.Element = element;
        }

        public static bool IsInstanceOf<T>() => Instance.Element is T;
    }
}
