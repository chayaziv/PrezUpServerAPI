using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrezUp.Core.Entity;
using PrezUp.Core.models;
using PrezUp.Core.Utils;

namespace PrezUp.Core.IServices
{
   public interface IAnalysisService
    {
        public Task<Result<Analysis>> AnalyzeAudioAsync(string filePath);
    }
}
