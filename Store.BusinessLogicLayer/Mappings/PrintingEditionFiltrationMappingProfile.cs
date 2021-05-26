using AutoMapper;
using Store.BusinessLogicLayer.Models.EditionModel;
using Store.DataAccessLayer.FiltrationModels;

namespace Store.BusinessLogicLayer.Mappings
{
    public class PrintingEditionFiltrationMappingProfile : Profile
    {
        public PrintingEditionFiltrationMappingProfile()
        {
            CreateMap<EditionFiltrationModelDAL, EditionFiltrationModel>().ReverseMap();
        }
    }
}

