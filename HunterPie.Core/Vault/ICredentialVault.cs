using HunterPie.Core.Vault.Model;

namespace HunterPie.Core.Vault;
public interface ICredentialVault
{
    public void Create(string username, string password);
    public Credential? Get(string username);
    public void Delete(string username);
}
