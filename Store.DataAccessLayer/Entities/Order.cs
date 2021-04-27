using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Store.DataAccessLayer.Enums.Enums.OrderStatysState;

namespace Store.DataAccessLayer.Entities
{
    public class Order : BaseEntity
    {
        public string Discription { get; set; }
        [ForeignKey("User")]
        public long UserId { get; set; }
        [ForeignKey("Payment")]
        public long PaymentId { get; set; }
        public DateTime Date { get; set; }
        public OrderStastys Status { get; set; }
        public IEnumerable<OrderItem> OrderItems { get; set; }

    }
}
