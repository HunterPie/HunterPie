namespace HunterPie.Features.Extensions.Entity;

internal record ThemeManifest(
    string Id,
    string Name,
    string Description,
    string Version,
    string Author,
    string[] Tags
);