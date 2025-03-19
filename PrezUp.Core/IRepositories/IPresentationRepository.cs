using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrezUp.Core.Entity;

namespace PrezUp.Core.IRepositories
{
    public interface IPresentationRepository : IRepository<Presentation>
    {
        public Task<List<Presentation>> GetPresentationsByUserIdAsync(int userId);
    
        public Task<List<Presentation>> GetPublicPresentationsAsync();
        Task<int> GetTotalPresentationsAsync();
        public Task<int> GetPublicPresentationsCountAsync();
    }
}
;