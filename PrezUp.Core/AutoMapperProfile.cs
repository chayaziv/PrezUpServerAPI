
using AutoMapper;
using BCrypt.Net;
using PrezUp.Core.Entity;
using PrezUp.Core.EntityDTO;

namespace PrezUp.Core
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
           CreateMap<Tag, TagDTO>().ReverseMap();
            CreateMap<PresentationDTO, Presentation>();
            CreateMap<Presentation, PresentationDTO>()
            .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags));

            CreateMap<Analysis, Presentation>();

            CreateMap<User, UserDTO>()
                    .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.PasswordHash)); // הצגת הסיסמא המוצפנת ב-DTO

            // הצפנת הסיסמא בזמן המרת ה-DTO חזרה ל-Entity
            CreateMap<UserDTO, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => EncryptPassword(src.Password)));
            CreateMap<Role, RoleDTO>();

            CreateMap<User, UserAdminDTO>()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Roles.Any() ? src.Roles.First() : new Role() { Id = 0, RoleName = "unknown" }))
                    .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.PasswordHash))
                     .ReverseMap()
                     .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => EncryptPassword( src.Password)));
    


            CreateMap<UserAdminDTO, UserDTO>();
           

        }


        // פונקציה להצפנת סיסמא
        private string EncryptPassword(string password)
        {
            // שימוש ב-BCrypt כדי להצפין את הסיסמא
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
    }
}
