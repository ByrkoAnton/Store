using System.Collections.Generic;
using static Store.DataAccessLayer.Enums.Enums;
using Dapper.Contrib.Extensions;

namespace Store.DataAccessLayer.Entities
{
    public class Order : BaseEntity
    {
        public string Discription { get; set; }//TODO wrong spelling 
        public  long UserId { get; set; }
        [Computed]
        public virtual User User { get; set; }
        public long PaymentId { get; set; }
        public OrderStatusState Status { get; set; }
        [Computed]
        public virtual List<OrderItem> OrderItems { get; set; }

        public Order()
        {
            Status = OrderStatusState.Unpayed;
        }
    }
}
