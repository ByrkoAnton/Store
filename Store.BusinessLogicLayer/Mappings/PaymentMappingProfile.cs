using AutoMapper;
using Store.BusinessLogicLayer.Models.Payments;
using Store.DataAccessLayer.Entities;

namespace Store.BusinessLogicLayer.Mappings
{
    public class PaymentMappingProfile: Profile
    {
        public PaymentMappingProfile()
        {
            CreateMap<Payment, PaymentCreationModel>().ReverseMap();
        }
    }
}
