using HunterPie.Core.Address.Map;
using HunterPie.Core.Client;
using HunterPie.Core.Domain.Enums;
using HunterPie.Core.Logger;
using HunterPie.Core.System.Windows.Exceptions;
using System.Diagnostics;
using System.IO;
using System.Xml;

namespace HunterPie.Core.System.Windows;

#nullable enable
internal class MHRProcessManager : WindowsProcessManager
{
    private const string GamePassManifestFile = "appxmanifest.xml";
    private const string GamePassVersionXPath = "//appx:Package/appx:Identity";
    private const string GamePassVersionKey = "Version";
    public override string Name => "MonsterHunterRise";
    public override GameProcess Game => GameProcess.MonsterHunterRise;

    protected override bool ShouldOpenProcess(Process process)
    {
        if (!process.MainWindowTitle.ToUpperInvariant().StartsWith("MONSTER HUNTER RISE"))
            return false;

        string riseVersion;
        try
        {
            riseVersion = DetectVersion(process);
        }
        catch (UnsupportedGameVersion err)
        {
            Log.Error(err.ToString());
            ShouldPollProcess = false;
            return false;
        }
        catch
        {
            Log.Error("Failed to get Monster Hunter Rise version, missing permissions. Try running as administrator.");
            ShouldPollProcess = false;
            return false;
        }

        Log.Info($"Detected Monster Hunter Rise version: {riseVersion}");

        _ = AddressMap.Parse(Path.Combine(ClientInfo.AddressPath, $"MonsterHunterRise.{riseVersion}.map"));

        if (!AddressMap.IsLoaded)
        {
            Log.Error("Failed to load address for Monster Hunter Rise v{0}", riseVersion);
            ShouldPollProcess = false;
            return false;
        }

        return true;
    }

    private string DetectVersion(Process process)
    {
        string? version = DetectSteamVersion(process)
                          ?? DetectGamePassVersion(process);

        return version ?? throw new UnsupportedGameVersion();
    }

    private string? DetectGamePassVersion(Process process)
    {
        if (process.MainModule is not { } module)
            return null;

        string processPath = Path.GetDirectoryName(module.FileName)!;
        string manifestPath = Path.Combine(processPath, GamePassManifestFile);

        if (!File.Exists(manifestPath))
            return null;

        var manifest = new XmlDocument();
        manifest.Load(manifestPath);

        var namespaceManager = new XmlNamespaceManager(manifest.NameTable);
        namespaceManager.AddNamespace("appx", "http://schemas.microsoft.com/appx/manifest/foundation/windows10");

        XmlNode? node = manifest.SelectSingleNode(GamePassVersionXPath, namespaceManager);
        if (node?.Attributes?[GamePassVersionKey]?.Value is { } version)
            return $"GamePass.{version}";

        return null;
    }

    private string? DetectSteamVersion(Process process)
    {
        if (process.MainModule is not { } module)
            return null;

        return module.FileVersionInfo.FileVersion;
    }
}
