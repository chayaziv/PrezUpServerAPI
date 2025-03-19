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

        public async Task<List<Presentation>> GetPresentationsByUserIdAsync(int userId)
        {
            return await _dbSet.Where(pres => pres.UserId == userId).ToListAsync();
        }
        public async Task<List<Presentation>> GetPublicPresentationsAsync()
        {
            return await _dbSet.Where(pres => pres.IsPublic).ToListAsync();
        }


        public async Task<int> GetTotalPresentationsAsync() => await _dbSet.CountAsync();
        public async Task<int> GetPublicPresentationsCountAsync() => await _dbSet.CountAsync(p => p.IsPublic);
        //public async Task<List<TopUserDto>> GetTopUsersAsync()
        //{
        //    return await _dbSet
        //        .OrderByDescending(u => u.Presentations.Count)
        //        .Take(5)
        //        .Select(u => new TopUserDto { UserId = u.Id, UserName = u.Name, PresentationsCount = u.Presentations.Count() })
        //        .ToListAsync();
        //}



    }
}
