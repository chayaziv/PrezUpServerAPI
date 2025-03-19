using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrezUp.Core.Entity;
using PrezUp.Core.EntityDTO;

namespace PrezUp.Core.IRepositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<bool> ExistsByEmailAsync(string email);
        Task<User?> GetByEmailAsync(string email);

        Task<List<User>> GetUsersWithRoles();
        Task<User?> GetByIdAsync(int id, bool includeRoles = false);

        Task<int> GetTotalUsersAsync() ;
        Task<int> GetActiveUsersAsync() ;
        Task<int> GetInactiveUsersAsync() ;

        Task<List<UserActivityDto>> GetUserActivityAsync();
        Task<List<TopUserDto>> GetTopUsersAsync();
        Task<List<UnusualActivityDto>> GetUnusualActivityAsync();




    }
}
