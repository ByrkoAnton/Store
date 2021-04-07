using AutoMapper;
using Store.BusinessLogicLayer.Models.Authors;
using Store.BusinessLogicLayer.Models.EditionModel;
using Store.BusinessLogicLayer.Models.Users;
using Store.DataAccessLayer.Entities;
using System.Linq;

namespace Store.BusinessLogicLayer.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserUpdateModel>().ReverseMap();

            //CreateMap<PrintingEditionModel, PrintingEdition>()
            //    .ForMember(x => x.Authors, opt => opt
            //    .MapFrom(src => src.AuthorModels));

            //CreateMap<PrintingEdition, PrintingEditionModel>()
            //    .ForMember(x => x.AuthorModels, opt => opt.Ignore());

            //CreateMap<AuthorModel, Author>()
            //    .ForMember(x => x.PrintingEditions, opt => opt.Ignore());

            //CreateMap<Author, AuthorModel>()
            //    .ForMember(x => x.PrintingEditionModels, opt => opt
            //    .MapFrom(src => src.PrintingEditions));



            CreateMap<PrintingEditionModel, PrintingEdition>().ForMember(x => x.Authors, opt => opt.MapFrom(src => src.AuthorModels));
            CreateMap<PrintingEdition, PrintingEditionModel>();

            CreateMap<AuthorModel, Author>().ForMember(x => x.PrintingEditions, opt => opt.MapFrom(src => src.PrintingEditionModels));
            CreateMap<Author, AuthorModel>().ForMember(x => x.PrintingEditionModels, opt => opt.MapFrom(src => src.PrintingEditions));


            //CreateMap<PrintingEdition, PrintingEditionModel>().ReverseMap();
            //CreateMap<PrintingEdition, PrintingEditionModel>().ForMember(i => i.AuthorModels, opt => opt
            //.MapFrom(src => src.Authors));

            //CreateMap<PrintingEditionModel, PrintingEdition>().ForMember(i => i.Authors, opt => opt
            //.MapFrom(src => src.AuthorModels));

            //CreateMap<Author, AuthorModel>().ForMember(i => i.PrintingEditionModels, opt => opt
            //.MapFrom(src => src.PrintingEditions));
            ////CreateMap<Author, AuthorModel>().ForMember(i => i.PrintingEditionModels, opt => opt
            ////.MapFrom(src => src.PrintingEditions.Select(y => y.Authors.ToList())));

            //CreateMap<AuthorModel, Author>().ForMember(x => x.PrintingEditions, opt => opt.Ignore());
            ////CreateMap<PrintingEdition, PrintingEditionModel>()
            ////    .ForMember(i => i.AuthorModels, opt => opt
            ////    .MapFrom(d => d.Authors
            ////    .Select(y => y.PrintingEditions)
            ////    .ToList()));

        }
    }
}
