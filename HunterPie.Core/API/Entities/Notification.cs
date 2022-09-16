using Newtonsoft.Json;
using System;

namespace HunterPie.Core.API.Entities
{
    public class Notification
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("icon")]
        public string Icon { get; set; }

        [JsonProperty("notification_type")]
        public NotificationType NotificationType { get; set; }

        [JsonProperty("primary_action")]
        public string PrimaryAction { get; set; }

        [JsonProperty("secondary_action")]
        public string SecondaryAction { get; set; }

        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }
    }
}
