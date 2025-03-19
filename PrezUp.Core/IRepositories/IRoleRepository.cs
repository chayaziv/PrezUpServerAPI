using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrezUp.Core.Entity;
using PrezUp.Core.EntityDTO;

namespace PrezUp.Core.IRepositories
{
    public interface IRoleRepository : IRepository<Role>
    {
        Task<Role> GetByNameAsync(string roleName);
        Task<List<RoleDistributionDto>> GetRolesDistributionAsync();
    }
}
