using HunterPie.Core.Client;
using HunterPie.Update.Presentation;
using System.Threading.Tasks;

namespace HunterPie.Update.Usecase;

internal class UpdateService : IUpdateUseCase
{
    public async Task<bool> Invoke()
    {
        if (!ClientConfig.Config.Client.EnableAutoUpdate)
            return false;

        UpdateViewModel vm = new();
        var view = new UpdateView { DataContext = vm };

        if (!ClientConfig.Config.Client.EnableSeamlessStartup)
            view.Show();

        bool result = await UpdateUseCase.Exec(vm);

        view.Close();
        return result;
    }
}