using Store.BusinessLogicLayer.Models.OrderItems;
using System.Collections.Generic;
namespace Store.BusinessLogicLayer.Models.Stipe
{
    public class StripePayModel
    {
        public string CardNumber { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public string Cvc { get; set; }
        public List<EditionIdAndQuantModel> EditionsIdAndQuant { get; set; }
        public string OrderDescription { get; set; }
        public long userId { get; set; }
        public int Value { get; set; }
        public string Jwt { get; set; }
    }
}
