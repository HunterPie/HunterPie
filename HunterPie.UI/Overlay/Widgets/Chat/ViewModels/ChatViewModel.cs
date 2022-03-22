using HunterPie.Core.Architecture;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HunterPie.UI.Overlay.Widgets.Chat.ViewModels
{
    public class ChatViewModel : Bindable
    {
        public ObservableCollection<ChatCategoryViewModel> Categories { get; } = new();


    }
}
