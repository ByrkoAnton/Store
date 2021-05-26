using System.Collections.Generic;
using static Store.DataAccessLayer.Enums.Enums.EditionEnums;

namespace Store.DataAccessLayer.Entities
{
    public class PrintingEdition : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public bool IsRemoved { get; set; }
        public string Status { get; set; }
        public Currency Currency { get; set; }
        public PrintingEditionType Type { get; set; }
        public virtual List<Author> Authors { get; set; }
        public PrintingEdition()
        {
            Authors = new List<Author>();
        }
    }
}
