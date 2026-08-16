namespace HunterPie.Features.Plugins.Entity;

internal record class AvailablePlugin(
    string Name,
    string Registry,
    string[] Releases
);
