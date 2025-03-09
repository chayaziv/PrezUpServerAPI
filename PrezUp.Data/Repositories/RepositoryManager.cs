using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PrezUp.Core.Entity;
using PrezUp.Core.IRepositories;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
namespace PrezUp.Data.Repositories
{
   public class RepositoryManager:IRepositoryManager
    {
        private readonly DataContext _context;
        public IPresentationRepository Presentations { get; }

        public IRepository<User> Users { get; }

        public RepositoryManager(DataContext context, IPresentationRepository presentations, IRepository<User> users)
        {
            _context = context;
            Presentations = presentations;
            Users = users;
        }
        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task<Presentation> SavePresentationAsync(AnalysisResult analysisResult)
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
                UserId=2
                
            };
            Presentations.AddAsync(presentation);
            return presentation;
        }
    }
}
