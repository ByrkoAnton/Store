using Store.DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.DataAccessLayer.SupportingClasses
{
    public class EntitiesWithCount<T> 
    {
        public IEnumerable<T> Entities { get; set; }
        public int Count { get; set; }
    }
}
