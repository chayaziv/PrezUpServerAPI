using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrezUp.Core.Entity
{
    [Table("Presentation")]
    public class Presentation
    {
        [Key]
        public int Id { get; set; }
        public string  Title { get; set; }= string.Empty;
        public string? FileUrl { get; set; } = string.Empty;
        public int Clarity { get; set; }
        public string ClarityFeedback { get; set; }
        public int Fluency { get; set; }
        public string FluencyFeedback { get; set; }
        public int Confidence { get; set; }
        public string ConfidenceFeedback { get; set; }
        public int Engagement { get; set; }
        public string EngagementFeedback { get; set; }

        public int SpeechStyle { get; set; }
        public string SpeechStyleFeedback { get; set; }

        public int Score { get; set; }
        public string Tips { get; set; }

        public bool IsPublic { get; set; }
        // קשר למשתמש
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }

    }
}
