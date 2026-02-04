using HunterPie.Core.Client;
using HunterPie.Core.Crypto;
using HunterPie.Core.Domain.Cache;
using HunterPie.Core.Zip.Service;
using HunterPie.DI.Module;
using HunterPie.Internal.Logger;
using System.Windows;

namespace HunterPie.DI.Modules;

internal class HunterPieModule : IDependencyModule
{
    public void Register(IDependencyRegistry registry)
    {
        // Intrinsic
        registry
            .WithSingle(_ => Application.Current.Dispatcher)
            .WithSingle(_ => Application.Current)
            .WithSingle<CryptoService>()
            .WithFactory<InMemoryAsyncCache>()
            .WithFactory<CompressorService>()
            .WithSingle<FileStreamLogWriter>()
            .WithSingle(static _ => ClientConfig.Config);
    }
}