using AutoMapper;
using Certify.Models;

namespace Certify.ViewModels.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            this.CreateMap<Document, ForMyDocumentsInfo>();
            this.CreateMap<ForMyDocumentsInfo, Document>();
            this.CreateMap<Document, ForMyDocumentsIndex>()
               .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
               .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
               .ForMember(dest => dest.FileURL, opt => opt.MapFrom(src => src.FileURL));
            this.CreateMap<ForMyDocumentsCreate, Document>();
        }
    }
}
