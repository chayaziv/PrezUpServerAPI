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


    }
}
