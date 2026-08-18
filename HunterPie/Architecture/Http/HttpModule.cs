using HunterPie.Architecture.Http.Factory;
using HunterPie.Architecture.Http.Interceptors;
using HunterPie.DI;
using HunterPie.DI.Module;
using System;
using System.Net.Http;

namespace HunterPie.Architecture.Http;

internal class HttpModule : IDependencyModule
{
    public void Register(IDependencyRegistry registry)
    {
        registry
            .WithSingle(_ =>
            {
                var handler = new SocketsHttpHandler
                {
                    PooledConnectionLifetime = TimeSpan.FromMinutes(5)
                };

                var client = new HttpClient(new DefaultHeadersInterceptor(handler));

                return client;
            })
            .WithSingle<DefaultHttpClientFactory>();
    }
}
