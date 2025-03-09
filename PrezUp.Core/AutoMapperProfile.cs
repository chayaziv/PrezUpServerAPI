
using AutoMapper;
using PrezUp.Core.Entity;
using PrezUp.Core.EntityDTO;

namespace PrezUp.Core
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Presentation, PresentationDTO>().ReverseMap();
            CreateMap<User, UserDTO>().ReverseMap();       
        }
    }
}
