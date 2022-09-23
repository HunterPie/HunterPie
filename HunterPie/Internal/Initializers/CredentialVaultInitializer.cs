using HunterPie.Core.System.Common.Exceptions;
using HunterPie.Core.System.Windows.Vault;
using HunterPie.Core.Vault;
using HunterPie.Domain.Interfaces;
using System.Runtime.InteropServices;

namespace HunterPie.Internal.Initializers;
internal class CredentialVaultInitializer : IInitializer
{
    public void Init()
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            throw new UnsupportedPlatformException();

        var credentialVault = new WindowsCredentialVault();

        _ = new CredentialVaultService(credentialVault);
    }
}
