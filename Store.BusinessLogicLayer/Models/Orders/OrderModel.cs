using Store.BusinessLogicLayer.Models.Base;
using Store.BusinessLogicLayer.Models.OrderItems;
using System.Collections.Generic;
using static Store.DataAccessLayer.Enums.Enums;

namespace Store.BusinessLogicLayer.Models.Orders
{
    public class OrderModel:BaseModel
    {
        public string Description { get; set; }//TODO wrong spelling
        public long UserId { get; set; }
        public long PaymentId { get; set; }
        public OrderStatusState Status { get; set; }
        public List<OrderItemModel> OrderItems { get; set; }
    }
}
