using HunterPie.Core.Architecture;

namespace HunterPie.UI.Architecture
{
    public interface IView<TViewModel> where TViewModel : Notifiable
    {
        public TViewModel Model { get; }
    }
}
