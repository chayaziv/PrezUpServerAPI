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
    public class RoleRepository : Repository<Role>, IRoleRepository
    {
        public RoleRepository(DataContext context) : base(context)
        {
        }

        public async Task<Role?> GetByNameAsync(string roleName)
        {
            return await _dbSet
                .Where(r => r.RoleName.ToLower() == roleName.ToLower()) // המרה לאותיות קטנות
                .FirstOrDefaultAsync();
        }
        public async Task<List<RoleDistributionDto>> GetRolesDistributionAsync()
        {
            return await _dbSet
                .Select(r => new RoleDistributionDto { RoleName = r.RoleName, UserCount = r.Users.Count() })
                .ToListAsync();
        }
    }
}
