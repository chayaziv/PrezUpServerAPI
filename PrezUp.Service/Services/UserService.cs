
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
using PrezUp.Core.Utils;

namespace PrezUp.Service.Services
{
    public class UserService : IUserService
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly IPresentationService _presentationService;
        private readonly IValidatorService _validator;

        public UserService(IRepositoryManager repository, IMapper mapper, IPresentationService presentationService, IValidatorService validator)
        {
            _repository = repository;
            _mapper = mapper;
            _presentationService = presentationService;
            _validator = validator;
        }

        public async Task<Result<List<UserDTO>>> GetAllAsync()
        {
            var list = await _repository.Users.GetListAsync();
            if (list == null || !list.Any())
                return Result<List<UserDTO>>.NotFound("No users found");

            return Result<List<UserDTO>>.Success(_mapper.Map<List<UserDTO>>(list));
        }

        public async Task<Result<UserDTO>> GetByIdAsync(int id)
        {
            var item = await _repository.Users.GetByIdAsync(id);
            if (item == null)
                return Result<UserDTO>.NotFound("User not found");

            return Result<UserDTO>.Success(_mapper.Map<UserDTO>(item));
        }

        public async Task<Result<UserDTO>> AddAsync(UserDTO user)
        {
            var validatorResult = await _validator.ValidateUserAsync(user);
            if (!validatorResult.IsValid)
                return Result<UserDTO>.BadRequest(validatorResult.Message);
            var model = _mapper.Map<User>(user);
            await _repository.Users.AddAsync(model);
            await _repository.SaveAsync();
            return Result<UserDTO>.Success(_mapper.Map<UserDTO>(model));
        }

        public async Task<Result<UserDTO>> UpdateAsync(int id, UserDTO user)
        {
            var existingUser = await _repository.Users.GetByIdAsync(id);
            if (existingUser == null)
                return Result<UserDTO>.NotFound("User not found");

            var validatorResult = await _validator.ValidateUserAsync(user);
            if (!validatorResult.IsValid)
                return Result<UserDTO>.BadRequest(validatorResult.Message);
          
            var model = _mapper.Map<User>(user);
            _repository.Users.UpdateAsync(model);
            await _repository.SaveAsync();
            return Result<UserDTO>.Success(_mapper.Map<UserDTO>(model));
        }

        public async Task<Result<bool>> DeleteAsync(int id)
        {
            var user = await _repository.Users.GetByIdAsync(id);
            if (user == null)
                return Result<bool>.NotFound("User not found");

            var presentations = await _repository.Presentations.GetPresentationsByUserIdAsync(id);
            var success = true;
            foreach (var presentation in presentations)
            {
              var res= await _presentationService.deleteAsync(presentation.Id, user.Id);
                if (!res.IsSuccess)
                    success=false ;
            }
            _repository.Users.DeleteAsync(user);
            if (await _repository.SaveAsync() == 0)
                return Result<bool>.Failure("Error eccured when delete user from DB");
            if (success)
                return Result<bool>.SuccessNoContent();

            return Result<bool>.BadRequest("not all presentations were deleted");
        }

        public async Task<Result<List<PresentationDTO>>> GetPresentationsByUserIdAsync(int userId)
        {
            var presentations = await _repository.Presentations.GetPresentationsByUserIdAsync(userId);
            if (presentations == null || !presentations.Any())
                return Result<List<PresentationDTO>>.SuccessNoContent();

            return Result<List<PresentationDTO>>.Success(_mapper.Map<List<PresentationDTO>>(presentations));
        }
    }
}
