using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrezUp.Core.EntityDTO;


namespace PrezUp.Core.models
{
    public class AuthType
    {
       
        public string Token { get; set; }

        public UserDTO User { get; set; }
    }
}
