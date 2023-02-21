using HunterPie.Core.Architecture;
using HunterPie.Core.Client;
using HunterPie.Core.Client.Events;
using HunterPie.Core.Domain.Enums;
using HunterPie.Integrations.Poogie.Common.Models;
using HunterPie.Integrations.Poogie.Version;
using HunterPie.Integrations.Poogie.Version.Models;
using HunterPie.UI.Controls.Settings.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.RegularExpressions;

namespace HunterPie.GUI.Parts.Settings.ViewModels;

public class SettingHostViewModel : Bindable
{
    private readonly PoogieVersionConnector _versionConnector = new();

    private int _currentTabIndex;
    private bool _isFetchingVersion;
    private bool _isLatestVersion;
    private string _lastSync = DateTime.Now.ToString("G");
    private readonly HashSet<GameProcess> _ignorableGames = new() { GameProcess.None, GameProcess.All };

    public ObservableCollection<ISettingElement> Elements { get; } = new();
    public ObservableCollection<GameProcess> Games { get; } = new();
    public Observable<GameProcess> SelectedGame => ClientConfig.Config.Client.LastConfiguredGame;
    public int CurrentTabIndex { get => _currentTabIndex; set => SetValue(ref _currentTabIndex, value); }
    public bool IsFetchingVersion { get => _isFetchingVersion; set => SetValue(ref _isFetchingVersion, value); }
    public bool IsLatestVersion { get => _isLatestVersion; set => SetValue(ref _isLatestVersion, value); }
    public string LastSync { get => _lastSync; set => SetValue(ref _lastSync, value); }

    public SettingHostViewModel(ISettingElement[] elements)
    {
        ConfigManager.OnSync += OnConfigSync;

        foreach (ISettingElement el in elements)
            Elements.Add(el);

        foreach (GameProcess gameType in Enum.GetValues<GameProcess>())
            if (!_ignorableGames.Contains(gameType))
                Games.Add(gameType);
    }

    public void UnhookEvents() => ConfigManager.OnSync -= OnConfigSync;

    private void OnConfigSync(object sender, ConfigSaveEventArgs e)
    {
        if (Path.GetFileNameWithoutExtension(e.Path) != "config")
            return;

        LastSync = e.SyncedAt.ToString("G");
    }

    public void SearchSetting(string query)
    {
        ISettingElement tab = Elements[CurrentTabIndex];

        foreach (ISettingElementType field in tab.Elements)
            field.Match = Regex.IsMatch(field.Name, query, RegexOptions.IgnoreCase) || query.Length == 0;
    }


    public async void FetchVersion()
    {
        IsFetchingVersion = true;

        PoogieResult<VersionResponse> schema = await _versionConnector.Latest();

        if (schema.Response is { } resp)
        {
            var version = new Version(resp.LatestVersion);
            IsLatestVersion = ClientInfo.IsVersionGreaterOrEq(version);
        }

        IsFetchingVersion = false;
    }

    public void ExecuteRestart() => App.Restart();
}
