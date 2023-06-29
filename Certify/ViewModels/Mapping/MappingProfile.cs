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
        }
    }
}
