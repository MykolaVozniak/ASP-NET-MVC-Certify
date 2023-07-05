using AutoMapper;
using Certify.Library.ViewModels;
using Data.Entity;

namespace Certify.Services.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            this.CreateMap<Document, MyDocumentsInfoVM>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.Firstname + " " + src.User.Lastname));
            this.CreateMap<MyDocumentsInfoVM, Document>();
            this.CreateMap<Document, MyDocumentsIndexVM>();

            this.CreateMap<MyDocumentsCreateVM, Document>();

            this.CreateMap<User, MyDocumentsInfoVM>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Firstname + " " + src.Lastname));

            this.CreateMap<Document, MyDocumentsEditVM>();


        }
    }
}
