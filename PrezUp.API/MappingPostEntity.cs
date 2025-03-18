using AutoMapper;
using PrezUp.API.PostEntity;
using PrezUp.Core.EntityDTO;

namespace PrezUp.API
{
    public class MappingPostEntity : Profile
    {
        public MappingPostEntity()
        {
            //CreateMap<PresentationPost,PresentationDTO>();
            CreateMap<UserPost, UserDTO>().ReverseMap();

            CreateMap<UserAdminPost, UserAdminDTO>().ReverseMap();
        }
    }
}
