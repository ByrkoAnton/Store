using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.DataAccessLayer.Entities
{
    public class BaseEntity
    {
        public long Id { get; set; }

        

        public DateTime CreationDate { get; set; }

        public BaseEntity()
        {
            CreationDate = DateTime.Today;
        }
        
    }
}
