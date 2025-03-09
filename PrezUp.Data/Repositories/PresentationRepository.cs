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
    }
}
