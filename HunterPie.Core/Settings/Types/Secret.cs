using HunterPie.Core.Architecture;
using HunterPie.Core.Converters;
using HunterPie.Core.Crypto;
using HunterPie.DI;
using Newtonsoft.Json;

namespace HunterPie.Core.Settings.Types;

[JsonConverter(typeof(SecretConverter))]
public class Secret : Bindable
{
    private ICryptoService CryptoService => DependencyContainer.Get<ICryptoService>();

    [JsonIgnore]
    public string Value
    {
        get;
        set => SetValue(ref field, value);
    } = string.Empty;

    public string EncryptedValue
    {
        get => CryptoService.Encrypt(Value);
        set => Value = CryptoService.Decrypt(value);
    }

    public override string ToString() => Value;

    public static implicit operator string?(Secret? self) => self?.Value;
}