using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrezUp.Core.Entity;
using PrezUp.Core.EntityDTO;
using PrezUp.Core.Utils;

namespace PrezUp.Core.IServices
{
    public interface IUserService
    {
        Task<Result<List<UserAdminDTO>>> GetAllAsync();
        Task<Result<UserDTO>> GetByIdAsync(int id);
        Task<Result<UserAdminDTO>> AddAsync(UserAdminDTO user);
        Task<Result<UserAdminDTO>> UpdateAdminAsync(int id, UserAdminDTO user);
        Task<Result<bool>> DeleteAsync(int id);

        Task<Result<List<PresentationDTO>>> GetPresentationsByUserIdAsync(int userId);
    }
}
