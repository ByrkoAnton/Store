using Store.BusinessLogicLayer.Models.Base;
using static Store.DataAccessLayer.Enums.Enums.EditionEnums;

namespace Store.BusinessLogicLayer.Models.OrderItems
{
    public class OrderItemModel : BaseModel
    {
        public double EditionPrice { get; set; }
        public CurrencyType Currency { get; set; }
        public long PrintingEditionId { get; set; }
        public long OrderId { get; set; }
        public int Count { get; set; }
    }
}
