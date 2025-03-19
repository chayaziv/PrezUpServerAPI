using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrezUp.Core.Entity
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("User")]
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [MaxLength(255)]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        // תחום עיסוק בהייטק
        public string? JobTitle{ get; set; } = string.Empty;
        public string? Company { get; set; } = string.Empty;
        public int? YearsOfExperience { get; set; }
            

        // אפשרויות משתמש
        public bool? CompareWithOthers { get; set; } = true;
        public bool? AllowPublicPresentations { get; set; } = false;

        
        // תיעוד סטטוס
       public DateOnly CreateAt { get; set; }
        public string? AccountStatus { get; set; } = "Active";

        public List<Presentation> Presentations { get; set; }
        public List<Role> Roles { get; set; } = new();
    }

    



}
