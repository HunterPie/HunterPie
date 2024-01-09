using HunterPie.Core.Client.Configuration.Enums;
using HunterPie.Core.Domain.Enums;
using HunterPie.Core.Settings.Adapters;
using System;

namespace HunterPie.Core.Client.Configuration.Adapters;

internal class TargetModeEnumAdapter : IEnumAdapter
{
    private static readonly Lazy<object[]> _riseValues = new(() => new object[] { TargetModeType.LockOn, TargetModeType.AutoQuest, TargetModeType.Infer });
    private static readonly Lazy<object[]> _worldValues = new(() => new object[] { TargetModeType.LockOn, TargetModeType.MapPin, TargetModeType.Infer });

    public object[] GetValues(GameProcess game)
    {
        return game switch
        {
            GameProcess.MonsterHunterRise => _riseValues.Value,
            GameProcess.MonsterHunterWorld => _worldValues.Value,
            GameProcess.None => Array.Empty<object>(),
            GameProcess.MonsterHunterRiseSunbreakDemo => Array.Empty<object>(),
            GameProcess.All => Array.Empty<object>(),
            _ => throw new ArgumentOutOfRangeException(nameof(game), game, null)
        };
    }
}
