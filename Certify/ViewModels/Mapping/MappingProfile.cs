using AutoMapper;
using Data.Entity;

namespace Certify.ViewModels.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            this.CreateMap<Document, ForMyDocumentsInfo>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.Firstname + " " + src.User.Lastname));
            this.CreateMap<ForMyDocumentsInfo, Document>();
            this.CreateMap<Document, ForMyDocumentsIndex>();
            this.CreateMap<ForMyDocumentsCreate, Document>();
           
            this.CreateMap<User, ForMyDocumentsInfo>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Firstname + " " + src.Lastname));

            this.CreateMap<Document, ForMyDocumentsEdit>();

        }
    }
}
