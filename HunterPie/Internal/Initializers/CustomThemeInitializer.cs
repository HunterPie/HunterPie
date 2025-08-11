using HunterPie.Core.Client;
using HunterPie.Domain.Interfaces;
using HunterPie.Features.Theme.Loader;
using System.IO;
using System.Threading.Tasks;

namespace HunterPie.Internal.Initializers;

internal class CustomThemeInitializer : IInitializer
{
    private readonly ThemeLoaderService _themeLoaderService;

    public CustomThemeInitializer(ThemeLoaderService themeLoaderService)
    {
        _themeLoaderService = themeLoaderService;
    }

    public Task Init()
    {
        string currentTheme = ClientConfig.Config.Client.Theme.Current;

        _themeLoaderService.Load(
            path: Path.Join(ClientInfo.ThemesPath, currentTheme)
        );

        return Task.CompletedTask;
    }
}