using HunterPie.Core.Client;
using HunterPie.Domain.Interfaces;
using HunterPie.Internal.Tray;
using System;
using System.Drawing;
using System.IO;

namespace HunterPie.Internal.Initializers
{
    internal class SystemTrayInitializer : IInitializer, IDisposable
    {
        public void Dispose() => TrayService.Dispose();

        public void Init()
        {
            TrayService.Initialize(
                "HunterPie",
                "HunterPie",
                Icon.ExtractAssociatedIcon(
                    Path.Combine(ClientInfo.ClientPath, "HunterPie.exe")
                )
            );
        }
    }
}
