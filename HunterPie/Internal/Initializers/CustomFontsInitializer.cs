using HunterPie.Core.Client;
using HunterPie.Core.Logger;
using HunterPie.Domain.Interfaces;
using HunterPie.UI.Platform.Windows.Native;
using System;
using System.IO;
using System.Linq;

namespace HunterPie.Internal.Initializers;

internal class CustomFontsInitializer : IInitializer, IDisposable
{
    private static readonly string _fontsFolder = ClientInfo.GetPathFor("Assets\\Fonts");

    private static readonly Lazy<string[]> _fonts = new(() =>
    {
        return !Directory.Exists(_fontsFolder)
            ? Array.Empty<string>()
            : Directory.EnumerateFiles(_fontsFolder)
                        .ToArray();
    });

    public void Init()
    {
        foreach (string fontName in _fonts.Value)
            _ = Gdi32.AddFontResourceW(fontName);

    }

    public void Dispose()
    {
        foreach (string fontName in _fonts.Value)
        {
            int result = Gdi32.RemoveFontResourceW(fontName);

            if (result == 0)
                Log.Error("Failed to remove font resource. Error code: {0}", result);
        }
    }
}
