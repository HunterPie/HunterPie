using HunterPie.Core.Address.Map;
using HunterPie.Core.Domain.Memory;
using HunterPie.Integrations.Datasources.MonsterHunterWilds.Definitions.Crypto;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;

namespace HunterPie.Integrations.Datasources.MonsterHunterWilds.Entity.Crypto;

public class MHWildsCryptoService : IDisposable
{
    private readonly IMemoryAsync _memory;
    private CryptographyVectors? _vectors;
    private readonly SemaphoreSlim _semaphore = new(1, 1);

    public MHWildsCryptoService(IMemoryAsync memory)
    {
        _memory = memory;
    }

    public async Task<float> DecryptFloat(MHWildsEncryptedFloat encrypted)
    {
        CryptographyVectors vectors = await GetCryptographyVectors();

        Vector128<byte> values = vectors.Key ^ encrypted.ToVector128();
        Vector128<byte> decryptedValues = Aes.DecryptLast(values, vectors.Round);

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