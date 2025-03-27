using System;

namespace PrezUp.Core.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string UserId { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? Link { get; set; }
        public NotificationType Type { get; set; }
    }

    public enum NotificationType
    {
        General,
        PresentationFeedback,
        SystemAlert,
        Achievement
    }
} 