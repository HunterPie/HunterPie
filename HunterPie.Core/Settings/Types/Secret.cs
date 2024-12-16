using HunterPie.Core.Architecture;
using HunterPie.Core.Converters;
using HunterPie.Core.Crypto;
using Newtonsoft.Json;

namespace HunterPie.Core.Settings.Types;

#nullable enable
[JsonConverter(typeof(SecretConverter))]
public class Secret : Bindable
{
    private string? _value;

    [JsonIgnore]
    public string? Value
    {
        get => _value;
        set => SetValue(ref _value, value);
    }

    public string EncryptedValue
    {
        get => CryptoService.Encrypt(Value);
        set => Value = CryptoService.Decrypt(value);
    }

    public override string ToString() => Value;

    public static implicit operator string?(Secret? self) => self?.Value;
}