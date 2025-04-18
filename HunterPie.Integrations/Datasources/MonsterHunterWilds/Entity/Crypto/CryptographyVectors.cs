using System.Runtime.Intrinsics;

namespace HunterPie.Integrations.Datasources.MonsterHunterWilds.Entity.Crypto;

internal record CryptographyVectors(
    Vector128<byte> Key,
    Vector128<byte> Round
);