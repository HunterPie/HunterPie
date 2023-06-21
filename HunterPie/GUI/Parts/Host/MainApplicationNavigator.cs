using HunterPie.Core.Architecture;
using HunterPie.UI.Architecture.Navigator;
using System.Windows;

namespace HunterPie.GUI.Parts.Host;

internal class MainApplicationNavigator : Bindable, INavigator
{
    private static MainApplicationNavigator? _instance;
    public static MainApplicationNavigator Instance => _instance ??= new();

    private UIElement? _lastElement;
    private UIElement? _element;

    public UIElement? Element { get => _element; set => SetValue(ref _element, value); }

    private MainApplicationNavigator() { }

    public void Navigate<T>(T element, bool forceRefresh) where T : UIElement
    {
        if (!forceRefresh && IsInstanceOf<T>())
            return;

        _lastElement = Element;
        Element = element;
    }

    public void Return()
    {
        if (_lastElement is null)
            return;

        Element = _lastElement;
        _lastElement = null;
    }

    public bool IsInstanceOf<T>() => Element is T;
}
