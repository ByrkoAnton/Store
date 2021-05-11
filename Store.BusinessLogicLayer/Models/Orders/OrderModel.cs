using Store.BusinessLogicLayer.Models.OrderItem;
using System.Collections.Generic;
using static Store.DataAccessLayer.Enums.Enums;

namespace Store.BusinessLogicLayer.Models.Orders
{
    public class OrderModel
    {
        public long Id { get; set; }
        public string Discription { get; set; }
        public long UserId { get; set; }
        public long PaymentId { get; set; }
        public OrderStastys Status { get; set; }
        public IEnumerable<OrderItemModel> OrderItemModels { get; set; }
    }
}
