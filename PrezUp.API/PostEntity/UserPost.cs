using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrezUp.API.PostEntity
{
    
    public class UserPost
    {      
           
        public string Name { get; set; } = string.Empty;
      
        public string Email { get; set; } = string.Empty;
       
        public string Password { get; set; } = string.Empty;
     
        public string JobTitle { get; set; } = string.Empty;
        public string Company { get; set; } = string.Empty;
        public int YearsOfExperience { get; set; }
                 
        public bool CompareWithOthers { get; set; } = true;
        public bool AllowPublicPresentations { get; set; } = false;
           
        public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
        public string AccountStatus { get; set; } = "Active";
       
    }

}
