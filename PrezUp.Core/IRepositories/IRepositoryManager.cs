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
        public IRepository <PresentationDTO> Presentations { get; }
        public IRepository<UserDTO> Users { get; }

        Task<int> SaveAsync();

        Task<PresentationDTO> SavePresentationAsync(AnalysisResult analysis);
    }
}
