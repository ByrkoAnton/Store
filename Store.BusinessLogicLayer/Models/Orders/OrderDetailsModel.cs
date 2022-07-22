using System;
using System.Collections.Generic;

namespace Store.BusinessLogicLayer.Models.Orders
{
    public class OrderDetailsModel
    {
        public long OrderId { get; set; }
        public DateTime DateOfCreation { get; set; }
        public long PaymentId { get; set; }
        public string OrderStatus { get; set; }
        public string Description { get; set; }
        public long UserId { get; set; }
        public string UserName { get; set; }
        public List<EditionInOrderDetails> Editions { get; set; }
        public double TotalPrice { get; set; }

    }

    public class EditionInOrderDetails//TODO wrong spelling+++
    {
        public long EditionId { get; set; }
        public string EditionTitle { get; set; }
        public double EditionPrice { get; set; }
        public int Count { get; set; }
        public double PriceForAllEditions { get; set; }
        public EditionInOrderDetails()
        {
            PriceForAllEditions = EditionPrice * Count;
        }
    }
}
