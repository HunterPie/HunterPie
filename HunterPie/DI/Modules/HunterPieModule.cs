using HunterPie.Core.Client;
using HunterPie.Core.Crypto;
using HunterPie.Core.Domain.Cache;
using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Observability.Logging;
using HunterPie.Core.Zip;
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
            .WithSingle<ICryptoService>(static r => new CryptoService(
                localRegistry: r.Get<ILocalRegistry>()
            ))
            .WithFactory<IAsyncCache>(static _ => new InMemoryAsyncCache())
            .WithFactory<ICompressor>(static _ => new CompressorService())
            .WithSingle<ILogWriter>(static _ => new FileStreamLogWriter())
            .WithSingle(static _ => ClientConfig.Config);
    }
}