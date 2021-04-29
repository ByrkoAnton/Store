using System.Collections.Generic;
using static Store.DataAccessLayer.Enums.Enums;

namespace Store.DataAccessLayer.Entities
{
    public class PrintingEdition : BaseEntity
    {
        public string Description { get; set; }
        public double Prise { get; set; }
        public bool IsRemoved { get; set; }
        public string Status { get; set; }
        public Currency Currency { get; set; }
        public PrintingEditionType Type { get; set; }
        public List<Author> Authors { get; set; }
        public PrintingEdition()
        {
            Authors = new();
        }
    }
}
