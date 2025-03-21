﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrezUp.Core.Entity
{
    public class Analysis
    {
        //public int Clarity { get; set; }
        //public string ClarityFeedback { get; set; }
        //public int Fluency { get; set; }
        //public string FluencyFeedback { get; set; }
        //public int Confidence { get; set; }
        //public string ConfidenceFeedback { get; set; }
        //public int Engagement { get; set; }
        //public string EngagementFeedback { get; set; }
        //public int SpeechStyle { get; set; }
        //public string SpeechStyleFeedback { get; set; }
        //public int Score { get; set; }
        //public string Tips { get; set; }
        ////----------------
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

    }
}
