using HunterPie.Core.Vault.Model;

namespace HunterPie.Core.Vault;
internal class CredentialVaultService
{
    private static ICredentialVault _vault;

    public CredentialVaultService(ICredentialVault vault)
    {
        _vault = vault;
    }

    public static void SaveCredential(string username, string password) => _vault.Create(username, password);
    public static void DeleteCredential(string username) => _vault.Delete(username);
    public static Credential? GetCredential(string username) => _vault.Get(username);

}
