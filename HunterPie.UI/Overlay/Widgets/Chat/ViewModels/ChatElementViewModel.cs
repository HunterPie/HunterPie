using HunterPie.Core.Architecture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace HunterPie.UI.Overlay.Widgets.Chat.ViewModels
{
    public class ChatElementViewModel : Bindable
    {
        private string _text;
        private string _author;
        private Brush _color;

        public string Text { get => _text; set { SetValue(ref _text, value); } }
        public string Author { get => _author; set { SetValue(ref _author, value); } }
        public Brush Color { get => _color; set { SetValue(ref _color, value); } }

    }
}
