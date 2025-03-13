﻿using System;
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
        Task<Result<List<UserDTO>>> GetAllAsync();
        Task<Result<UserDTO>> GetByIdAsync(int id);
        Task<Result<UserDTO>> AddAsync(UserDTO user);
        Task<Result<UserDTO>> UpdateAsync(int id, UserDTO user);
        Task<Result<bool>> DeleteAsync(int id);

        Task<Result<List<PresentationDTO>>> GetPresentationsByUserIdAsync(int userId);
    }
}
