using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrezUp.Core.models;

namespace PrezUp.Core.IServices
{
   public interface IAudioAnalysisService
    {
        public Task<AudioResult> AnalyzeAudioAsync(string filePath);
    }
}
