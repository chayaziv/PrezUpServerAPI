using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrezUp.Core.Entity;

namespace PrezUp.Core.IRepositories
{
    public interface IRoleRepository : IRepository<Role>
    {
        public Task<Role> GetByNameAsync(string roleName);
    }
}
