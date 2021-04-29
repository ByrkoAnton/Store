using Store.BusinessLogicLayer.Models.Base;
using System.ComponentModel.DataAnnotations;


namespace Store.BusinessLogicLayer.Models.Payments
{
    public class PaymentModel : BaseModel
    {
        [Required]
        public string TransactionId { get; set; }
    }
}
