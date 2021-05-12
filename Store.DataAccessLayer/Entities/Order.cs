using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using static Store.DataAccessLayer.Enums.Enums;

namespace Store.DataAccessLayer.Entities
{
    public class Order : BaseEntity
    {
        public string Discription { get; set; }
        [ForeignKey("User")]
        public long UserId { get; set; }
        [ForeignKey("Payment")]
        public long PaymentId { get; set; }
        public OrderStatus Status { get; set; }
        public IEnumerable<OrderItem> OrderItems { get; set; }
    }
}
