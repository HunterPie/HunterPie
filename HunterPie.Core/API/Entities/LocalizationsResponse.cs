using Newtonsoft.Json;
using System.Collections.Generic;

namespace HunterPie.Core.API.Entities;
public class LocalizationsResponse
{
    [JsonProperty("localizations")]
    public Dictionary<string, string> Localizations { get; set; }
}
