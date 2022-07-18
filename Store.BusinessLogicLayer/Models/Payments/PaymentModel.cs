using Store.BusinessLogicLayer.Models.Base;
using System.ComponentModel.DataAnnotations;
//TODO extra line

namespace Store.BusinessLogicLayer.Models.Payments
{
    public class PaymentModel : BaseModel//TODO are u using this model?
    {
        [Required]
        public string TransactionId { get; set; }
    }
}
