using HunterPie.UI.Controls.Settings.ViewModel;
using HunterPie.UI.Settings;
using System;
using System.Threading.Tasks;

namespace HunterPie.Features.Account.Config;
internal class LocalAccountConfig
{
    private static LocalAccountConfig _instance;
    private AccountConfig AccountConfig { get; } = new();

    private static LocalAccountConfig Instance
    {
        get
        {
            _instance ??= new();

            return _instance;
        }
    }

    public static AccountConfig Config => Instance.AccountConfig;

    public static async Task<ISettingElement[]> CreateAccountSettingsTab()
    {
        bool isLoggedIn = await AccountManager.ValidateSessionToken();

        if (!isLoggedIn)
            return Array.Empty<ISettingElement>();

        return VisualConverterManager.Build(Instance);
    }
}
