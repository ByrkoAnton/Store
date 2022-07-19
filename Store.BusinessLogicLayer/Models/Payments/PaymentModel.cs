using Store.BusinessLogicLayer.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace Store.BusinessLogicLayer.Models.Payments
{
    public class PaymentModel : BaseModel//TODO are u using this model?
    {
        [Required]
        public string TransactionId { get; set; }
    }
}
