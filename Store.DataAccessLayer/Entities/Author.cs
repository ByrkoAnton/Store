using System.Collections.Generic;
using Dapper.Contrib.Extensions;//TODO add new line
namespace Store.DataAccessLayer.Entities
{
    public class Author : BaseEntity
    {
        public string Name { get; set; }
        [Computed]
        public virtual List<PrintingEdition> PrintingEditions { get; set; }
        public Author()
        {
            PrintingEditions = new List<PrintingEdition>();
        }
    }
}
