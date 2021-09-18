using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace HunterPie.UI.Architecture.Extensions
{
    static class INotifyPropertyChangedExtensions
    {
        static public void N(this INotifyPropertyChanged self, PropertyChangedEventHandler @event, [CallerMemberName] string propertyName = "")
        {
            @event?.Invoke(self, new PropertyChangedEventArgs(propertyName));
        }
    }
}
