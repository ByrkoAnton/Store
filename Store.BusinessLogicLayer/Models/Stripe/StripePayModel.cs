using Store.BusinessLogicLayer.Models.OrderItems;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Store.BusinessLogicLayer.Models.Stripe
{
    public class StripePayModel
    {
        [Required]
        [CreditCard]
        public string CardNumber { get; set; }
        [Required]
        public int CardExpMonth { get; set; }
        [Required]
        public int CardExpYear { get; set; }
        [Required]
        public string CardCvc { get; set; }
        [Required]
        public List<EditionPayModel> Editions { get; set; }
        public string OrderDescription { get; set; }
    }
}
