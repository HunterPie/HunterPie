using HunterPie.Features.Account.Config;
using HunterPie.Features.Account.Controller;
using HunterPie.Update;
using System.Threading.Tasks;

namespace HunterPie;

internal class MainApplication
{
    private readonly AccountController _accountController;
    private readonly IUpdateUseCase _updateUseCase;
    private readonly IRemoteAccountConfigUseCase _remoteAccountConfigUseCase;

    public MainApplication(
        AccountController accountController,
        IUpdateUseCase updateUseCase,
        IRemoteAccountConfigUseCase remoteAccountConfigUseCase
        )
    {
        _accountController = accountController;
        _updateUseCase = updateUseCase;
        _remoteAccountConfigUseCase = remoteAccountConfigUseCase;
    }

    public async Task Start()
    {
        await _updateUseCase.Invoke();

        return;
    }
}