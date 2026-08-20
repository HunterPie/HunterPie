using HunterPie.Core.Json;
using HunterPie.Core.Networking.Http.Models;
using HunterPie.Core.Observability.Logging;
using System;
using System.Net;
using System.Threading.Tasks;

namespace HunterPie.Integrations.Poogie.Common.Models;

internal record PoogieResult<T>(
    T? Response,
    PoogieError? Error
)
{
    private static readonly ILogger Logger = LoggerFactory.Create();

    public static async Task<PoogieResult<T>> FromAsync(Response.Success response)
    {
        try
        {
            string content = await response.TextAsync();

            if (response.StatusCode >= HttpStatusCode.BadRequest)
                return new PoogieResult<T>(
                    Response: default,
                    Error: JsonProvider.Deserializer<PoogieError>(content)
                );

            return new PoogieResult<T>(
                Response: JsonProvider.Deserializer<T>(content),
                Error: null
            );
        }
        catch (Exception ex)
        {
            Logger.Error($"Failed to read content from response: {ex}");

            return new PoogieResult<T>(
                Response: default,
                Error: PoogieError.Default()
            );
        }
        finally
        {
            response.Dispose();
        }
    }
}