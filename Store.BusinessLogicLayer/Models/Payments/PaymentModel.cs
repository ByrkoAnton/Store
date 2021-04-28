using Store.BusinessLogicLayer.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.BusinessLogicLayer.Models.Payments
{
    public class PaymentModel : BaseModel
    {
        [Required]
        public string TransactionId { get; set; }
    }
}
