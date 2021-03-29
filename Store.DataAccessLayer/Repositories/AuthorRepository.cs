using Microsoft.EntityFrameworkCore;
using Store.DataAccessLayer.Entities;
using Store.DataAccessLayer.Repositories.Base;
using Store.DataAccessLayer.Repositories.Interfaces;

namespace Store.DataAccessLayer.Repositories
{
    public class AuthorRepository : BaseRepository<Author>, IAuthorRepository
    {
        public AuthorRepository(DbContext context) : base(context)
        {
        }
    }
}
