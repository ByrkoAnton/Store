namespace Store.DataAccessLayer.Dapper.HelperClasses
{
    public class AuthorIdEditionId
    {
        public long AuthorId { get; set; }
        public long EditionId { get; set; }

        public AuthorIdEditionId(long _authorId, long _editionId)
        {
            AuthorId = _authorId;
            EditionId = _editionId;
        }
    }
}
