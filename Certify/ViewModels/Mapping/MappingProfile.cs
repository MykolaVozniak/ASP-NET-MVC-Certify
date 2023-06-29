using AutoMapper;
using Certify.Models;

namespace Certify.ViewModels.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            this.CreateMap<Document, DocumentViewModel>();
            this.CreateMap<DocumentViewModel, Document>();
            this.CreateMap<Document, DocumentIndex>()
               .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
               .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
               .ForMember(dest => dest.FileURL, opt => opt.MapFrom(src => src.FileURL));
        }
    }
}
