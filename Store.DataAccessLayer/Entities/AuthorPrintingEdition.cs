
namespace Store.DataAccessLayer.Entities
{
    public class AuthorPrintingEdition
    {
        public long AuthorId { get; set; }
        public Author Author { get; set; }

        public long PrintingEditionId { get; set; }
        public PrintingEdition PrintingEdition { get; set; }
    }
}
