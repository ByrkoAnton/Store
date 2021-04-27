using System.ComponentModel.DataAnnotations.Schema;
using static Store.DataAccessLayer.Enums.Enums.PrintingEditionEnums;

namespace Store.DataAccessLayer.Entities
{
    public class OrderItem : BaseEntity
    {
        public double Amount { get; set; }
        public Currency Currency { get; set; }
        public long PrintingEditionId { get; set; }
        [ForeignKey ("Order")]
        public long OrderId { get; set; }
        public int Count { get; set; }

    }
}