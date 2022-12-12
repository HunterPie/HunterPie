using HunterPie.Core.Json;
using HunterPie.Core.Logger;
using HunterPie.Core.Networking.Http;
using System.Threading.Tasks;

namespace HunterPie.Integrations.Poogie.Common.Models;

#nullable enable
internal record PoogieResult<T>(
    T? Response,
    PoogieError? Error
)
{
    public static async Task<PoogieResult<T>> From(HttpClientResponse response)
    {
        string rawResponse = await response.AsTextAsync();

        var resp = default(T);
        PoogieError? error = null;
        try
        {
            error = JsonProvider.Deserializer<PoogieError>(rawResponse);
        }
        catch
        { }

        if (error is null || error.Code == PoogieErrorCode.NOT_ERROR)
            try
            {
                resp = JsonProvider.Deserializer<T>(rawResponse);
                error = null;
            }
            catch
            {
                Log.Error("Failed to deserialize response body to JSON");
            }

        return new PoogieResult<T>(Response: resp, Error: error);
    }
}
