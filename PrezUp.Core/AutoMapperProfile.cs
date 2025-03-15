
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
            CreateMap<Presentation, PresentationDTO>().ReverseMap();
            
            CreateMap<Analysis, Presentation>();

            CreateMap<User, UserDTO>()
                    .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.PasswordHash)); // הצגת הסיסמא המוצפנת ב-DTO

            // הצפנת הסיסמא בזמן המרת ה-DTO חזרה ל-Entity
            CreateMap<UserDTO, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => EncryptPassword(src.Password)));


        }


        // פונקציה להצפנת סיסמא
        private string EncryptPassword(string password)
        {
            // שימוש ב-BCrypt כדי להצפין את הסיסמא
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
    }
}
