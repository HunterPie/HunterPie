using HunterPie.UI.Architecture;

namespace HunterPie.UI.Navigation.Service;

public interface INavigationRegistry
{
    public INavigationRegistry Bind<TActivity, TViewModel>()
        where TActivity : Activity
        where TViewModel : ViewModel;

}