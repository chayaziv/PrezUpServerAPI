using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using PrezUp.Core.Entity;
using PrezUp.Core.EntityDTO;
using PrezUp.Core.IRepositories;
using PrezUp.Core.IServices;

namespace PrezUp.Service.Services
{
   public  class UserService : IUserService
   {
        private readonly IRepositoryManager _repository;
        readonly IMapper _mapper;
        public UserService(IRepositoryManager repository,IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<UserDTO>> GetAllAsync()
        {
            var list = await _repository.Users.GetListAsync();
           
           return _mapper.Map<List<UserDTO>>(list);
        }

        public async Task<UserDTO> GetByIdAsync(int id)
        {
            var item = await _repository.Users.GetByIdAsync(id);
            return _mapper.Map<UserDTO>(item);
        }

        public async Task<UserDTO> AddAsync(UserDTO user)
        {
            var model =  _mapper.Map<User>(user);
            await _repository.Users.AddAsync(model);
            await _repository.SaveAsync();
            return _mapper.Map<UserDTO>(model);
        }

        public async Task<UserDTO> UpdateAsync(int id, UserDTO user)
        {
            var model = _mapper.Map<User>(user);
            var updated = _repository.Users.UpdateAsync(model);
            await _repository.SaveAsync();
            return _mapper.Map<UserDTO>(updated);
        }

        //public async Task<bool> DeleteAsync(int id)
        //{
        //    User itemToDelete = await _repository.Users.GetByIdAsync(id);
        //    _repository.Users.DeleteAsync(itemToDelete);
        //    await _repository.SaveAsync();
        //    return true;
        //}
        public async Task<bool> DeleteAsync(int id)
        {
            var user = await _repository.Users.GetByIdAsync(id);
            if (user == null)
                return false;

            // מחיקת כל הפרזנטציות של המשתמש
            var presentations = await _repository.Presentations.GetPresentationsByUserIdAsync(id);
            foreach (var presentation in presentations)
            {
                _repository.Presentations.DeleteAsync(presentation);
            }

            // מחיקת המשתמש עצמו
            _repository.Users.DeleteAsync(user);

            // שמירת השינויים
            await _repository.SaveAsync();

            return true;
        }

        public async Task<List<PresentationDTO>> GetPresentationsByUserIdAsync(int userId)
        {
            var presentations=  await _repository.Presentations.GetPresentationsByUserIdAsync(userId);
            return _mapper.Map<List<PresentationDTO>>(presentations);
        }
        
    }
}
