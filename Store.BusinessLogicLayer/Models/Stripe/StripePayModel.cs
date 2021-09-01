using Store.BusinessLogicLayer.Models.OrderItems;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Store.BusinessLogicLayer.Models.Stripe
{
    public class StripePayModel
    {
        public string Token { get; set; }
        [Required]
        public List<EditionPayModel> Editions { get; set; }
        public string OrderDescription { get; set; }
    }
}
