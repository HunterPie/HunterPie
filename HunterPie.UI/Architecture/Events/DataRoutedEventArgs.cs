using System.Windows;

namespace HunterPie.UI.Architecture.Events;

public class DataRoutedEventArgs<T>(RoutedEvent routedEvent, object source, T data) : RoutedEventArgs(routedEvent, source)
{
    public T Data { get; } = data;
}