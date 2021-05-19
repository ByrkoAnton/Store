using System.Collections.Generic;
using static Store.DataAccessLayer.Enums.Enums;
namespace Store.DataAccessLayer.Entities
{
    public class Order : BaseEntity
    {
        public string Discription { get; set; }
        public  long UserId { get; set; }
        public virtual User User { get; set; }
        public long PaymentId { get; set; }
        public OrderStatus Status { get; set; }
        public virtual List<OrderItem> OrderItems { get; set; }

        public Order()
        {
            Status = OrderStatus.Unpayed;
        }
    }
}
