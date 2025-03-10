using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrezUp.Core.models;

namespace PrezUp.Core.IServices
{
    public interface IAuthService
    {

        Task<AuthResult> RegisterUserAsync(RegisterModel model);
        Task<string> LoginAsync(LoginModel model);
    }
}
