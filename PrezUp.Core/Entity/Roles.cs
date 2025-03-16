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
    //[Table("UserRole")]
    //public class UserRole
    //{
    //    public int UserId { get; set; }
    //    public User User { get; set; } = null!;
    //    public int RoleId { get; set; }
    //    public Role Role { get; set; } = null!;
    //}
    //[Table("RolePermission")]
    //public class RolePermission
    //{
    //    public int RoleId { get; set; }

    //    public Role Role { get; set; } = null!;
    //    public int PermissionId { get; set; }
    //    public Permission Permission { get; set; } = null!;
    //}
}
