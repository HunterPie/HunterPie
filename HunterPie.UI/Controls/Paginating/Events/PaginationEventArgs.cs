using System.Windows;

namespace HunterPie.UI.Controls.Paginating.Events;

public class PaginationEventArgs(RoutedEvent routedEvent, object sender, int page) : RoutedEventArgs(routedEvent, sender)
{
    /// <summary>
    /// The page that was clicked
    /// </summary>
    public int Page { get; } = page;
}