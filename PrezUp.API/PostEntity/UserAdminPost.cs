using PrezUp.Core.EntityDTO;

namespace PrezUp.API.PostEntity
{
    public class UserAdminPost
    {
       
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
