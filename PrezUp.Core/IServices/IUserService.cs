using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrezUp.Core.Entity;

namespace PrezUp.Core.IServices
{
    public interface IUserService
    {
        Task<List<UserDTO>> GetAllAsync();
        Task<UserDTO> GetByIdAsync(int id);
        Task<UserDTO> AddAsync(UserDTO user);
        Task<UserDTO> UpdateAsync(int id, UserDTO user);
        Task<bool> DeleteAsync(int id);
    }
}
