using Store.BusinessLogicLayer.Models.Base;
using static Store.DataAccessLayer.Enums.Enums;

namespace Store.BusinessLogicLayer.Models.OrderItem
{
    public class OrderItemModel : BaseModel
    {
        public double Amount { get; set; }
        public Currency Currency { get; set; }
        public long PrintingEditionId { get; set; }
        public long OrderId { get; set; }
        public int Count { get; set; }
    }
}
