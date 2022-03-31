using HunterPie.Core.Architecture;
using HunterPie.Core.Client;
using HunterPie.Core.Client.Events;
using HunterPie.Core.Http;
using HunterPie.Core.Logger;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.RegularExpressions;

namespace HunterPie.UI.Controls.Settings.ViewModel
{
    public class SettingHostViewModel : Bindable
    {
        private int _currentTabIndex;
        private bool _isFetchingVersion;
        private bool _isLatestVersion;
        public string _lastSync = DateTime.Now.ToString("G");
        private readonly ObservableCollection<ISettingElement> _elements = new();

        public ObservableCollection<ISettingElement> Elements => _elements;
        public int CurrentTabIndex { get => _currentTabIndex; set { SetValue(ref _currentTabIndex, value); } }
        public bool IsFetchingVersion { get => _isFetchingVersion; set { SetValue(ref _isFetchingVersion, value); } }
        public bool IsLatestVersion { get => _isLatestVersion; set { SetValue(ref _isLatestVersion, value); } }
        public string LastSync { get => _lastSync; set { SetValue(ref _lastSync, value); } }

        public SettingHostViewModel(ISettingElement[] elements)
        {
            ConfigManager.OnSync += OnConfigSync;

            foreach (ISettingElement el in elements)
                _elements.Add(el);
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

        // TODO: move this out of here when API code is ready
        private struct VersionResponseSchema
        {
            [JsonProperty("latest_version")]
            public string LatestVersion;
        }

        public async void FetchVersion()
        {
            IsFetchingVersion = true;

            using Poogie request = PoogieFactory.Default()
                                .Get("/v1/version")
                                .WithHeader("X-Supporter-Token", ClientConfig.Config.Client.SupporterSecretToken)
                                .WithTimeout(TimeSpan.FromSeconds(5))
                                .Build();

            using PoogieResponse resp = await request.RequestAsync();

            // TODO: Error status
            if (!resp.Success)
                return;

            VersionResponseSchema schema = await resp.AsJson<VersionResponseSchema>();
            Version version = new Version(schema.LatestVersion);

            IsLatestVersion = ClientInfo.IsVersionGreaterOrEq(version);

            IsFetchingVersion = false;
        }
    }
}
