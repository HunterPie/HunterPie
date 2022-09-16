using HunterPie.Core.Architecture;
using HunterPie.Core.Client;
using HunterPie.UI.Assets.Application;
using System.Diagnostics;
using System.Windows.Media;
using Localization = HunterPie.Core.Client.Localization.Localization;

namespace HunterPie.GUI.Parts.Sidebar.ViewModels;

internal class PatreonSideBarElementViewModel : Bindable, ISideBarElement
{
    private const string PATREON_URL = "https://www.patreon.com/HunterPie";
    private const string PATREON_HAS_CLICKED = "HasClickedPatreon";
    private bool _shouldNotify;

    public ImageSource Icon => Resources.Icon("ICON_PATREON");
    public string Text => Localization.QueryString("//Strings/Client/Tabs/Tab[@Id='PATREON_STRING']");
    public bool IsActivable => false;
    public bool IsEnabled => true;
    public bool ShouldNotify { get => _shouldNotify; set => SetValue(ref _shouldNotify, value); }
    public void ExecuteOnClick()
    {
        _ = Process.Start("explorer", PATREON_URL);
        RegistryConfig.Set(PATREON_HAS_CLICKED, true);

        ShouldNotify = false;
    }

    public PatreonSideBarElementViewModel()
    {
        if (!RegistryConfig.Exists(PATREON_HAS_CLICKED))
            RegistryConfig.Set(PATREON_HAS_CLICKED, false);

        bool hasClicked = RegistryConfig.Get<bool>(PATREON_HAS_CLICKED);

        ShouldNotify = !hasClicked;
    }
}
