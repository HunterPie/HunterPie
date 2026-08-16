using HunterPie.Core.Client;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace HunterPie.Architecture.Http.Interceptors;

internal class DefaultHeadersInterceptor : DelegatingHandler
{
    private const string UserAgentHeader = "User-Agent";
    private const string ClientTypeHeader = "X-HunterPie-Client";
    private const string SessionIdHeader = "X-Session-Id";
    private const string AppVersionHeader = "X-App-Version";

    private const string ClientType = "v2";
    private static readonly string UserAgent = GetUserAgent();
    private static readonly string AppVersion = ClientInfo.Version.ToString();
    private static readonly string Session = Guid.NewGuid().ToString();

    public DefaultHeadersInterceptor(HttpMessageHandler next)
    {
        InnerHandler = next;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        request.Headers.Add(UserAgentHeader, UserAgent);
        request.Headers.Add(SessionIdHeader, Session);
        request.Headers.Add(ClientTypeHeader, ClientType);
        request.Headers.Add(AppVersionHeader, AppVersion);

        return base.SendAsync(request, cancellationToken);
    }

    private static string GetUserAgent()
    {
        string platformVersion = Environment.OSVersion.ToString();
        return $"HunterPie/{AppVersion} ({platformVersion})";
    }
}
