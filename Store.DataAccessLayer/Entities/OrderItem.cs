using static Store.DataAccessLayer.Enums.Enums;

namespace Store.DataAccessLayer.Entities
{
    public class OrderItem : BaseEntity
    {
        public double Amount { get; set; }
        public Currency Currency { get; set; }
        public long PrintingEditionId { get; set; }
        public long OrderId { get; set; }
        public Order Order { get; set; }
        public int Count { get; set; } 
    }
}