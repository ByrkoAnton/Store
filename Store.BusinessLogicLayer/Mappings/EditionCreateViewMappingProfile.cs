using AutoMapper;
using Store.BusinessLogicLayer.Models.EditionModel;

namespace Store.BusinessLogicLayer.Mappings
{
    class EditionCreateViewMappingProfile : Profile
    {
        public EditionCreateViewMappingProfile()
        {
            CreateMap<EditionCreateViewModel, PrintingEditionModel>();

        }
    }
}

