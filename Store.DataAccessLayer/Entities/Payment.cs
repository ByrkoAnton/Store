using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.DataAccessLayer.Entities
{
    public class Payment : BaseEntity
    {
        public string TransactionId { get; set; }
    }
}
