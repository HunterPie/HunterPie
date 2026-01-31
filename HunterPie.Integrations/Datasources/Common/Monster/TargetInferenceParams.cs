namespace HunterPie.Integrations.Datasources.Common.Monster;

internal record struct TargetInferenceParams(
    DateTime LastHitAt,
    double HealthRatio,
    double Distance
);