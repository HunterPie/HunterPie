using Newtonsoft.Json;
using System;

namespace HunterPie.Integrations.Poogie.Notification.Models;

internal record NotificationResponse(
    [JsonProperty("title")] string Title,
    [JsonProperty("message")] string Message,
    [JsonProperty("icon")] string Icon,
    [JsonProperty("notification_type")] NotificationType NotificationType,
    [JsonProperty("primary_action")] string? PrimaryAction,
    [JsonProperty("secondary_action")] string? SecondaryAction,
    [JsonProperty("created_at")] DateTime CreatedAt
);