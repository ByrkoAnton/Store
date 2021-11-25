using Store.BusinessLogicLayer.Models.OrderItems;
using System.Collections.Generic;

namespace Store.BusinessLogicLayer.Models.Stripe
{
    public class StripePayModel
    {   
        public string Token { get; set; }
        public List<EditionPayModel> Editions { get; set; }
        public string OrderDescription { get; set; }
    }
}
