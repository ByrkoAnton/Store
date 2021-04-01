using System.Collections.Generic;
using static Store.DataAccessLayer.Enums.Enums.PrintingEditionEnums;

namespace Store.DataAccessLayer.Entities
{
    public class PrintingEdition
    {
        public long Id { get; set; }
        public string Description { get; set; }
        public double Prise { get; set; }
        public bool IsRemoved { get; set; }
        public string Status { get; set; }
        public Currency Currency { get; set; }
        public Type Type { get; set; }

        public List<Author> Authors { get; set; } = new();
    }
}
