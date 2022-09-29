namespace HunterPie.Core.API.Entities;

#nullable enable
public class PoogieApiResult<P>
{
    public bool Success { get; init; }
    public P? Response { get; init; }
    public ErrorResponse? Error { get; init; }
}
#nullable restore