using HunterPie.Core.Address.Map;
using HunterPie.Core.Domain.Memory;
using HunterPie.Integrations.Datasources.MonsterHunterWilds.Definitions.Crypto;
using System.Runtime.Intrinsics;
using X86Aes = System.Runtime.Intrinsics.X86.Aes;
namespace HunterPie.Integrations.Datasources.MonsterHunterWilds.Entity.Crypto;

public class MHWildsCryptoService(IMemoryAsync memory) : IDisposable
{
    private delegate Vector128<byte> AesFunc(Vector128<byte> values, Vector128<byte> roundKey);

    private readonly IMemoryAsync _memory = memory;
    private CryptographyVectors? _vectors;
    private readonly SemaphoreSlim _semaphore = new(1, 1);

    private readonly Lazy<AesFunc> _aesDecryptLast = new(static () =>
    {
        if (X86Aes.IsSupported)
            return X86Aes.DecryptLast;

        return ManualAesCrypto.DecryptLast;
    });

    public async Task<float> DecryptFloatAsync(MHWildsEncryptedFloat encrypted)
    {
        CryptographyVectors vectors = await GetCryptographyVectors();

        Vector128<byte> values = vectors.Key ^ encrypted.ToVector128();
        Vector128<byte> decryptedValues = _aesDecryptLast.Value(values, vectors.Round);

        return decryptedValues.AsSingle()[0];
    }

    private async Task<CryptographyVectors> GetCryptographyVectors()
    {
        try
        {
            await _semaphore.WaitAsync();

            if (_vectors is not null)
                return _vectors;

            ulong[] encryptionKey = await _memory.ReadAsync<ulong>(
                address: AddressMap.GetAbsolute("Encryption::Key"),
                count: 2
            );
            ulong[] roundKey = await _memory.ReadAsync<ulong>(
                address: AddressMap.GetAbsolute("Encryption::Round"),
                count: 2
            );

            _vectors = new CryptographyVectors(
                Key: Vector128.Create(encryptionKey)
                    .AsByte(),
                Round: Vector128.Create(roundKey)
                    .AsByte()
            );

            return _vectors;
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public void Dispose() => _semaphore.Dispose();
}