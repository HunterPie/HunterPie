using Newtonsoft.Json;
using System;

#nullable enable
namespace HunterPie.Core.API.Schemas
{
    public class Notification
    {
        [JsonProperty("title")]
        public string Title { get; set; } = string.Empty;

        [JsonProperty("message")]
        public string Message { get; set; } = string.Empty;

        [JsonProperty("icon")]
        public string Icon { get; set; } = string.Empty;

        [JsonProperty("notification_type")]
        public NotificationType NotificationType { get; set; }

        [JsonProperty("primary_action")]
        public string? PrimaryAction { get; set; }

        [JsonProperty("secondary_action")]
        public string? SecondaryAction { get; set; }

        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }
    }
}
#nullable restore
