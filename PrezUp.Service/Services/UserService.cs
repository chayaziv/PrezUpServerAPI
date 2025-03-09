using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrezUp.Core.Entity;
using PrezUp.Core.IRepositories;
using PrezUp.Core.IServices;

namespace PrezUp.Service.Services
{
   public  class UserService : IUserService
    {
        private readonly IRepositoryManager _repository;

        public UserService(IRepositoryManager repository)
        {
            _repository = repository;
        }

        public async Task<List<UserDTO>> GetAllAsync()
        {
            var list = await _repository.Users.GetListAsync();
            return list;
        }

        public async Task<UserDTO> GetByIdAsync(int id)
        {
            var item = await _repository.Users.GetByIdAsync(id);
            return item;
        }

        public async Task<UserDTO> AddAsync(UserDTO user)
        {
            await _repository.Users.AddAsync(user);
            await _repository.SaveAsync();
            return user;
        }

        public async Task<UserDTO> UpdateAsync(int id, UserDTO user)
        {
            var updated = _repository.Users.UpdateAsync(user);
            await _repository.SaveAsync();
            return updated;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            UserDTO itemToDelete = await _repository.Users.GetByIdAsync(id);
            _repository.Users.DeleteAsync(itemToDelete);
            await _repository.SaveAsync();
            return true;
        }
    }
}
