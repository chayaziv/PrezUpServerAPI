using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrezUp.Core.EntityDTO;


namespace PrezUp.Core.models
{
   public class AuthResult
    {
        public bool Succeeded { get; set; }
        public List<string> Errors { get; set; }

        public string Token { get; set; }   

        public UserDTO User { get; set; }
    }
}
