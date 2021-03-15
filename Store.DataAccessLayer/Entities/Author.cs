using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.DataAccessLayer.Entities
{
    public class Author
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }
}
