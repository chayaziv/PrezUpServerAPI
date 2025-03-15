using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrezUp.Core.EntityDTO;
using PrezUp.Core.Utils;

namespace PrezUp.Core.IServices
{
    public interface IValidatorService
    {
        Task<ValidResult> ValidateUserAsync(UserDTO user);
        bool ValidateName(string name);
        bool ValidateEmail(string email);
        bool ValidatePassword(string password);
        bool ValidateRequiredFields(UserDTO user);
        Task<bool> EmailExistsAsync(string email);
    }
}
