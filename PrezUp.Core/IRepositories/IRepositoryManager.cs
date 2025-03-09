using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrezUp.Core.Entity;

namespace PrezUp.Core.IRepositories
{
   public interface IRepositoryManager
    {
        public IRepository <Presentation> Presentations { get; }
        public IRepository<User> Users { get; }

        Task<int> SaveAsync();

        Task<Presentation> SavePresentationAsync(AnalysisResult analysis);
    }
}
