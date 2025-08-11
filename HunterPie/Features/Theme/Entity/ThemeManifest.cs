namespace HunterPie.Features.Theme.Entity;

internal record ThemeManifest(
    string Name,
    string Description,
    string Author,
    bool IsVerified,
    string[] AssetUrls
);