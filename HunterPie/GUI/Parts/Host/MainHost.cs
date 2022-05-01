using HunterPie.Core.Architecture;
using System.Windows;

namespace HunterPie.GUI.Parts.Host
{
    internal class MainHost : Bindable
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
                
        private MainHost() { }

        public UIElement Element { get => _element; set { SetValue(ref _element, value); } }

        public static void SetMain<T>(T element, bool forceRefresh = false) where T : UIElement
        {
            if (!forceRefresh && IsInstanceOf<T>())
                return;

            Instance.Element = element;
        }

        public static bool IsInstanceOf<T>() => Instance.Element is T;
    }
}
