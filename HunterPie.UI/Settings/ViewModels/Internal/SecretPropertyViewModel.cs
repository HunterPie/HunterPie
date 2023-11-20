using HunterPie.Core.Settings.Types;

namespace HunterPie.UI.Settings.ViewModels.Internal;

public class SecretPropertyViewModel : ConfigurationPropertyViewModel
{
    public Secret Secret { get; }

    public SecretPropertyViewModel(Secret secret)
    {
        Secret = secret;
    }
}