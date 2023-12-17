using HunterPie.UI.Architecture;

namespace HunterPie.UI.Navigation;

public interface INavigator
{
    public void Navigate<TViewModel>(TViewModel viewModel) where TViewModel : ViewModel;
}