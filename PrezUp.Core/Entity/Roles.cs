using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrezUp.Core.Entity
{
    [Table("Roles")]
    public class Role
    {
        public int Id { get; set; }
        public string RoleName { get; set; } = string.Empty;
        public List<User> Users { get; set; } = new();
        public List<Permission> Permissions { get; set; } = new();
    }

    [Table("Permission")]
    public class Permission
    {
        public int Id { get; set; }
        public string PermissionName { get; set; } = string.Empty;
        public List<Role> Roles { get; set; } = new();
    }
   
}
