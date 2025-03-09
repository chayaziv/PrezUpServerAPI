namespace PrezUp.API
{
    public class MappingPostEntity:Profile
    {
        public MappingPostModel()
        {
            CreateMap<AgreementPostModel, AgreementDTO>();
            CreateMap<CompanyPostModel, CompanyDTO>();
            CreateMap<DeliveryManPostModel, DeliveryManDTO>();
            CreateMap<SendingPostModel, SendingDTO>();
        }
    }
}
