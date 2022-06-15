using static Store.DataAccessLayer.Enums.Enums.EditionEnums;
using DapperAttribute = Dapper.Contrib.Extensions;

namespace Store.DataAccessLayer.Entities
{
    public class OrderItem : BaseEntity
    {
        public double EditionPrice { get; set; }
        public CurrencyType Currency { get; set; }
        public long PrintingEditionId { get; set; }
        public virtual long OrderId { get; set; }
        [DapperAttribute.Computed]
        public virtual Order Order { get; set; }
        public int Count { get; set; } 
    }
}