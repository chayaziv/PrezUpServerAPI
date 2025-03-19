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
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(DataContext context) : base(context)
        {
        }
        public async Task<bool> ExistsByEmailAsync(string email)
        {
            return await _dbSet.AnyAsync(u => u.Email == email);
        }


        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _dbSet
                .Include(u => u.Roles) 
                .FirstOrDefaultAsync(u => u.Email == email);
        }
        public async Task<List<User>> GetUsersWithRoles()
        {
            return await _dbSet.Include(u=>u.Roles).ToListAsync();
        }
        public async Task<User?> GetByIdAsync(int id, bool includeRoles = false)
        {
           

            if (includeRoles)
                return await  _dbSet.Include(u => u.Roles).FirstOrDefaultAsync(u => u.Id == id);

            return await _dbSet.FirstOrDefaultAsync(u => u.Id == id);
        }
        public async Task<int> GetTotalUsersAsync() => await _dbSet.CountAsync();
        public async Task<int> GetActiveUsersAsync() => await _dbSet.CountAsync(u => u.Presentations.Any());
        public async Task<int> GetInactiveUsersAsync() => await _dbSet.CountAsync(u => !u.Presentations.Any());

        public async Task<List<UserActivityDto>> GetUserActivityAsync()
        {
            return await _dbSet
                .Select(u => new UserActivityDto { UserId = u.Id, UserName = u.Name, PresentationCount = u.Presentations.Count() })
                .ToListAsync();
        }

        public async Task<List<TopUserDto>> GetTopUsersAsync()
        {
            return await _dbSet
                .OrderByDescending(u => u.Presentations.Count)
                .Take(5)
                .Select(u => new TopUserDto { UserId = u.Id, UserName = u.Name, PresentationsCount = u.Presentations.Count() })
                .ToListAsync();
        }

        public async Task<List<UnusualActivityDto>> GetUnusualActivityAsync()
        {
            return await _dbSet
                .Where(u => u.Presentations.Count > 10)
                .Select(u => new UnusualActivityDto { UserId = u.Id, UserName = u.Name, PresentationsCount = u.Presentations.Count() })
                .ToListAsync();
        }
    }
}
