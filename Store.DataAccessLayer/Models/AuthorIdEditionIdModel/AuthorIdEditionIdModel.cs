namespace Store.DataAccessLayer.Models.AuthorIdEditionIdModel
{
    public class AuthorIdEditionIdModel
    {
        public long AuthorId { get; set; }
        public long EditionId { get; set; }

        public AuthorIdEditionIdModel(long authorId, long editionId)
        {
            AuthorId = authorId;
            EditionId = editionId;
        }
    }
}