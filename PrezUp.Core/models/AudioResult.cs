using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrezUp.Core.Entity;

namespace PrezUp.Core.models
{
   public class AudioResult
    {


        public bool Succeeded { get; set; }
        public List<string> Errors { get; set; }

        public AnalysisResult analysis  { get; set; }

    }
}
