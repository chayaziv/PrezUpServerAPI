using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrezUp.Core.models
{
   public class AuthResult
    {
        public bool Succeeded { get; set; }
        public List<string> Errors { get; set; }

        public string Token { get; set; }   
    }
}
