using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrezUp.Core.Entity;

namespace PrezUp.Core.EntityDTO
{
    public class RoleDTO
    {
        public int Id { get; set; }
        public string RoleName { get; set; } = string.Empty;
    }
    public class UserAdminDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public string JobTitle { get; set; } = string.Empty;
        public string Company { get; set; } = string.Empty;
        public int YearsOfExperience { get; set; }

        public bool CompareWithOthers { get; set; } = true;
        public bool AllowPublicPresentations { get; set; } = false;

        public string AccountStatus { get; set; } = "Active";

        public RoleDTO Role { get; set; } = new();
    }
}
