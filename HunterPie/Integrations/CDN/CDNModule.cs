using HunterPie.Core.Networking.Http;
using HunterPie.DI;
using HunterPie.DI.Module;
using HunterPie.Integrations.CDN.Services;

namespace HunterPie.Integrations.CDN;

internal class CDNModule : IDependencyModule
{
    public void Register(IDependencyRegistry registry)
    {
        registry.WithSingle(r =>
        {
            return new ContentDeliveryNetworkService(
                client: r.Get<IHttpClientFactory>().Create(
                    urls: ["https://cdn.hunterpie.com"],
                    retry: 3,
                    interceptors: []
                )
            );
        });
    }
}
