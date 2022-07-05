namespace Store.DataAccessLayer.Dapper.HelperClasses
{
    public class AuthorIdEditionId
    {
        public long AuthorId { get; set; }
        public long EditionId { get; set; }

        public AuthorIdEditionId(long authorId, long editionId)
        {
            AuthorId = authorId;
            EditionId = editionId;
        }
    }
}
