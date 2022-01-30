using HunterPie.Core.Architecture;
using HunterPie.Core.Client;
using HunterPie.Core.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HunterPie.UI.Controls.Settings.ViewModel
{
    public class SettingHostViewModel : Bindable
    {
        private int _currentTabIndex;
        private bool _isFetchingVersion;
        private bool _isLatestVersion;
        private readonly ObservableCollection<ISettingElement> _elements = new();

        public ObservableCollection<ISettingElement> Elements => _elements;
        public int CurrentTabIndex { get => _currentTabIndex; set { SetValue(ref _currentTabIndex, value); } }
        public bool IsFetchingVersion { get => _isFetchingVersion; set { SetValue(ref _isFetchingVersion, value); } }
        public bool IsLatestVersion { get => _isLatestVersion; set { SetValue(ref _isLatestVersion, value); } }

        public SettingHostViewModel(IEnumerable<ISettingElement> elements)
        {
            foreach (ISettingElement el in elements)
                _elements.Add(el);
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
