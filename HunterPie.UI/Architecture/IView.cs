using HunterPie.Core.Architecture;

namespace HunterPie.UI.Architecture
{
    public interface IView<TViewModel> where TViewModel : Bindable
    {
        public TViewModel Model { get; }
    }
}
