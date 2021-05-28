using static Store.DataAccessLayer.Enums.Enums.EditionEnums;

namespace Store.DataAccessLayer.Entities
{
    public class OrderItem : BaseEntity
    {
        public double EditionPrice { get; set; }
        public Currency Currency { get; set; }
        public long PrintingEditionId { get; set; }
        public virtual long OrderId { get; set; }
        public virtual Order Order { get; set; }
        public int Count { get; set; } 
    }
}