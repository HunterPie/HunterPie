using HunterPie.Core.Domain.Features.Domain;
using HunterPie.Core.Domain.Features.Repository;
using System.Collections.Generic;

namespace HunterPie.Features.Flags.Repository;

internal class LocalFeatureFlagRepository(
    IReadOnlyDictionary<string, IFeature> flags
) : IFeatureFlagRepository
{
    public IFeature? GetFeature(string feature) => !flags.ContainsKey(feature) ? null : flags[feature];

    public bool IsEnabled(string feature) => flags.ContainsKey(feature) && (bool)flags[feature].IsEnabled;
}