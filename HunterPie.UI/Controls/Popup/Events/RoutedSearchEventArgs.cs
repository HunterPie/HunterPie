using System.Windows;

namespace HunterPie.UI.Controls.Popup.Events;

public class RoutedSearchEventArgs(RoutedEvent e, object sender) : RoutedEventArgs(e, sender)
{
    public required string Query { get; init; }
}