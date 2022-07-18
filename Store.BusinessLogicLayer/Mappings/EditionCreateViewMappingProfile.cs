using AutoMapper;
using Store.BusinessLogicLayer.Models.EditionModel;

namespace Store.BusinessLogicLayer.Mappings
{
    class EditionCreateViewMappingProfile : Profile //TODO modifier?
    {
        public EditionCreateViewMappingProfile()
        {
            CreateMap<EditionCreateViewModel, PrintingEditionModel>();

        }
    }
}

