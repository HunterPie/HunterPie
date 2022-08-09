using HunterPie.Core.API;
using HunterPie.Core.Architecture;
using HunterPie.Core.Client;
using HunterPie.Core.Client.Events;
using HunterPie.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;

namespace HunterPie.UI.Controls.Settings.ViewModel
{
    public class SettingHostViewModel : Bindable
    {
        private int _currentTabIndex;
        private bool _isFetchingVersion;
        private bool _isLatestVersion;
        public string _lastSync = DateTime.Now.ToString("G");
        private readonly ObservableCollection<ISettingElement> _elements = new();
        private readonly ObservableCollection<GameProcess> _games = new();
        private readonly HashSet<GameProcess> _ignorableGames = new() { GameProcess.None, GameProcess.All };

        public ObservableCollection<ISettingElement> Elements => _elements;
        public ObservableCollection<GameProcess> Games => _games;
        public Observable<GameProcess> SelectedGame => ClientConfig.Config.Client.LastConfiguredGame;
        public int CurrentTabIndex { get => _currentTabIndex; set { SetValue(ref _currentTabIndex, value); } }
        public bool IsFetchingVersion { get => _isFetchingVersion; set { SetValue(ref _isFetchingVersion, value); } }
        public bool IsLatestVersion { get => _isLatestVersion; set { SetValue(ref _isLatestVersion, value); } }
        public string LastSync { get => _lastSync; set { SetValue(ref _lastSync, value); } }

        public SettingHostViewModel(ISettingElement[] elements)
        {
            ConfigManager.OnSync += OnConfigSync;

            foreach (ISettingElement el in elements)
                _elements.Add(el);

            foreach (GameProcess gameType in Enum.GetValues<GameProcess>())
                if (!_ignorableGames.Contains(gameType))
                    _games.Add(gameType);
        }

        public void UnhookEvents()
        {
            ConfigManager.OnSync -= OnConfigSync;
        }

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

            var schema = await PoogieApi.GetLatestVersion();

            if (schema is not null)
            {
                Version version = new Version(schema.LatestVersion);
                IsLatestVersion = ClientInfo.IsVersionGreaterOrEq(version);
            }

            IsFetchingVersion = false;
        }

        public async void ExecuteRestart()
        {
            string path = Process.GetCurrentProcess().MainModule.FileName;
            Process.Start(path.Replace(".dll", ".exe"));
            Application.Current.Shutdown();
        }
    }
}
