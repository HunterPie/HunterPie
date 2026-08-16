using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace HunterPie.Core.Networking.Http.Intercept;

internal class HttpChain(
    IReadOnlyList<IHttpInterceptor> interceptors,
    Func<HttpRequestMessage, Task<HttpResponseMessage>> executeRequest
) : IHttpChain
{
    private int _current = 0;

    public async Task<HttpResponseMessage> NextAsync(HttpRequestMessage request)
    {
        if (_current >= interceptors.Count)
            return await executeRequest(request);

        IHttpInterceptor interceptor = interceptors[_current];
        _current++;

        return await interceptor.InterceptAsync(request, this);
    }
}
