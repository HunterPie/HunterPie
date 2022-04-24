using HunterPie.Core.Architecture;
using HunterPie.Core.Client;
using HunterPie.Core.Http;
using HunterPie.UI.Assets.Application;
using System;
using System.Diagnostics;
using System.Net;
using System.Windows;
using System.Windows.Media;
using Localization = HunterPie.Core.Client.Localization.Localization;

namespace HunterPie.GUI.Parts.Sidebar.ViewModels
{
    internal class PatchNotesSideBarElementViewModel : Bindable, ISideBarElement
    {
        private const string LAST_PATCH_NOTE_READ_KEY = "LastPatchNote";
        private bool _isEnabled;
        private bool _shouldNotify;
        private string _resolvedUrl;

        public ImageSource Icon => Resources.Icon("ICON_DOCUMENTATION");
        public string Text => Localization.Query("//Strings/Client/Tabs/Tab[@Id='PATCH_NOTES_STRING']").Attributes["String"].Value;
        public bool IsActivable => false;
        public bool IsEnabled { get => _isEnabled; private set { SetValue(ref _isEnabled, value); } }
        public bool ShouldNotify { get => _shouldNotify; private set { SetValue(ref _shouldNotify, value); } }

        public PatchNotesSideBarElementViewModel()
        {
            string clientVersion = ClientInfo.Version.ToString(3);
            Poogie poogie = PoogieFactory.Docs()
                .Get($"/posts/update-v{clientVersion}")
                .WithTimeout(TimeSpan.FromSeconds(10))
                .Build();

            poogie.RequestAsync()
                .ContinueWith((res) =>
                {
                    if (res.Result is PoogieResponse response)
                    {
                        if (!response.Success)
                            return;

                        IsEnabled = response.Status == HttpStatusCode.OK;
                        ShouldNotify = IsEnabled && RegistryConfig.Get<string>(LAST_PATCH_NOTE_READ_KEY) != response.Url;
                        _resolvedUrl = response.Url;
                    }
                });
        }

        public void ExecuteOnClick()
        {
            if (IsEnabled)
            {
                Process.Start("explorer", _resolvedUrl);
                ShouldNotify = false;
                RegistryConfig.Set(LAST_PATCH_NOTE_READ_KEY, _resolvedUrl);
            }
        }
    }
}
