
using System.Collections.Generic;


namespace Store.DataAccessLayer.Entities
{
    public class Author
    {
        public long Id { get; set; }
        public string Name { get; set; }

        public List<PrintingEdition> PrintingEditions { get; set; }

        public List<AuthorPrintingEdition> AuthorPrintingEditions { get; set; }

        public Author()
        {
            AuthorPrintingEditions = new List<AuthorPrintingEdition>();
        }

    }
}
