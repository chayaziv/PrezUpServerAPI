using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PrezUp.Core.Entity;
using PrezUp.Core.IRepositories;

namespace PrezUp.Data.Repositories
{
    public class PresentationRepository : Repository<Presentation>, IPresentationRepository
    {
        public PresentationRepository(DataContext context) : base(context)
        {
        }

        public async Task<List<Presentation>> GetPresentationsByUserIdAsync(int userId)
        {
            return await _dbSet.Where(pres => pres.UserId == userId).ToListAsync();
        }
        public async Task<List<Presentation>> GetPublicPresentationsAsync()
        {
            return await _dbSet.Where(pres => pres.IsPublic).ToListAsync();
        }


        public async Task<Presentation> SaveAnalysisAsync(AnalysisResult analysisResult, bool isPublic, int userId,string fileUrl)
        {
            Presentation presentation = new Presentation()
            {
                Clarity = analysisResult.Clarity,
                ClarityFeedback = analysisResult.ClarityFeedback,
                Fluency = analysisResult.Fluency,
                FluencyFeedback = analysisResult.FluencyFeedback,
                Confidence = analysisResult.Confidence,
                ConfidenceFeedback = analysisResult.ConfidenceFeedback,
                Engagement = analysisResult.Engagement,
                EngagementFeedback = analysisResult.EngagementFeedback,
                SpeechStyle = analysisResult.SpeechStyle,
                SpeechStyleFeedback = analysisResult.SpeechStyleFeedback,
                Score = analysisResult.Score,
                Tips = analysisResult.Tips,
                UserId = userId,
                IsPublic=isPublic,
                FileUrl=fileUrl

            };
            AddAsync(presentation);
            return presentation;
        }
    }
}
