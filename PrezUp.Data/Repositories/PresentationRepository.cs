using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PrezUp.Core.Entity;
using PrezUp.Core.EntityDTO;
using PrezUp.Core.IRepositories;

namespace PrezUp.Data.Repositories
{
    public class PresentationRepository : Repository<Presentation>, IPresentationRepository
    {
        public PresentationRepository(DataContext context) : base(context)
        {
        }

        public async Task<List<Presentation>> GetPresentationsByUserIdAsync(int userId) =>
            await _dbSet.Where(pres => pres.UserId == userId).ToListAsync();

        public async Task<List<Presentation>> GetPublicPresentationsAsync() =>
            await _dbSet.Where(pres => pres.IsPublic).ToListAsync();

        public async Task<int> GetTotalPresentationsAsync() =>
            await _dbSet.CountAsync();

        public async Task<int> GetPublicPresentationsCountAsync() =>
            await _dbSet.CountAsync(p => p.IsPublic);

        public async Task<List<Presentation>> GetPublicWithTagsAsync() =>
            await _dbSet.Include(p => p.Tags).Where(pres => pres.IsPublic).ToListAsync();
    }
}
