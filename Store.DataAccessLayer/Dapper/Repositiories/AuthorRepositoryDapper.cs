using Dapper;
using Dapper.Contrib.Extensions;
using Microsoft.Extensions.Options;
using Store.DataAccessLayer.Dapper.Interfaces;
using Store.DataAccessLayer.Entities;
using Store.DataAccessLayer.Models.FiltrationModels;
using Store.Sharing.Configuration;
using Store.Sharing.Constants;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Store.DataAccessLayer.Dapper.Repositiories
{
    public class AuthorRepositoryDapper : IAuthorRepositoryDapper
    {
        private readonly ConnectionStringConfig _options;
        public AuthorRepositoryDapper(IOptions<ConnectionStringConfig> options)
        {
            _options = options.Value;
        }
        public async Task<(IEnumerable<Author>, int)> GetAsync(AuthorFiltrationModelDAL model)
        {
            var skip = (model.CurrentPage - Constants.PaginationParams.DEFAULT_OFFSET) * model.PageSize;

            using (IDbConnection db = new SqlConnection(_options.DefaultConnection))
            {
                string sortDirection = model.IsAscending ? Constants.SortingParams.SORT_ASC : Constants.SortingParams.SORT_DESC;

                var queryGetAuthors = @"IF @propertyForSort = 'Name' AND @sortDirection = 'ASC'
                SELECT*
                FROM(
                SELECT*
                FROM Authors
                WHERE Authors.Name LIKE @nameForSearch
                ORDER BY Name ASC
                OFFSET @skip ROWS FETCH NEXT @pageSize ROWS ONLY )
                AS author
                LEFT JOIN AuthorPrintingEdition ON author.Id = AuthorPrintingEdition.AuthorsId 
                LEFT JOIN PrintingEditions ON AuthorPrintingEdition.PrintingEditionsId = PrintingEditions.Id
                ORDER BY author.Name ASC 

                IF @propertyForSort = 'Name' AND @sortDirection = 'DESC' 
                SELECT*
                FROM(
                SELECT *
                FROM Authors
                WHERE Authors.Name LIKE @nameForSearch
                ORDER BY Name DESC
                OFFSET @skip ROWS FETCH NEXT @pageSize ROWS ONLY)
                AS author
                LEFT JOIN AuthorPrintingEdition ON author.Id = AuthorPrintingEdition.AuthorsId 
                LEFT JOIN PrintingEditions ON AuthorPrintingEdition.PrintingEditionsId = PrintingEditions.Id 
                ORDER BY author.Name DESC


                IF @propertyForSort = 'Id' AND @sortDirection = 'ASC'
                SELECT*
                FROM(
                SELECT*
                FROM Authors
                WHERE Authors.Name LIKE @nameForSearch
                ORDER BY Id
                ASC OFFSET @skip ROWS FETCH NEXT @pageSize ROWS ONLY)
                AS author
                LEFT JOIN AuthorPrintingEdition ON author.Id = AuthorPrintingEdition.AuthorsId
                LEFT JOIN PrintingEditions ON AuthorPrintingEdition.PrintingEditionsId = PrintingEditions.Id
                ORDER BY author.Id ASC

                IF @propertyForSort = 'Id' AND @sortDirection = 'DESC'
                SELECT*
                FROM(
                SELECT*
                FROM Authors
                WHERE Authors.Name LIKE @nameForSearch
                ORDER BY Id DESC
                OFFSET @skip ROWS FETCH NEXT @pageSize ROWS ONLY) 
                AS author
                LEFT JOIN AuthorPrintingEdition ON author.Id = AuthorPrintingEdition.AuthorsId 
                LEFT JOIN PrintingEditions ON AuthorPrintingEdition.PrintingEditionsId = PrintingEditions.Id 
                ORDER BY author.Id DESC";

                var authorsParameters = new DynamicParameters();
                authorsParameters.Add("@propertyForSort", model.PropertyForSort);
                authorsParameters.Add("@skip", skip);
                authorsParameters.Add("@pageSize", model.PageSize);
                authorsParameters.Add("@nameForSearch", $"%{model.Name}%");
                authorsParameters.Add("@sortDirection", sortDirection);

                var authorDictionary = new Dictionary<long, Author>();

                var authors =
                    (await db.QueryAsync<Author, PrintingEdition, Author>(queryGetAuthors,
                    (author, editions) =>
                    {
                        Author authorEntry;
                        if (!authorDictionary.TryGetValue(author.Id, out authorEntry))
                        {
                            authorEntry = author;
                            authorEntry.PrintingEditions = new List<PrintingEdition>();
                            authorDictionary.Add(authorEntry.Id, authorEntry);
                        }
                        authorEntry.PrintingEditions.Add(editions);
                        return authorEntry;
                    },
                    authorsParameters)).Distinct().ToList();

                var countParameters = new DynamicParameters();
                countParameters.Add("@nameForSearch", $"%{model.Name}%");

                var queryGetAuthorsCount = "SELECT COUNT (Authors.Id) FROM Authors WHERE Name LIKE @nameForSearch";
                int authorsCount = (await db.QueryAsync<int>(queryGetAuthorsCount, countParameters)).FirstOrDefault();

                var authorsWithCount = (authors: authors, count: authorsCount);
                return authorsWithCount;
            }
        }

        public async Task<List<Author>> GetAuthorsListByNamesListAsync(List<string> names)
        {
            using IDbConnection db = new SqlConnection(_options.DefaultConnection);
            var query = @"SELECT *
                            FROM
                            Authors WHERE Authors.Name IN @authors
                            ORDER BY Name";

            var parameters = new DynamicParameters();
            parameters.Add("@authors", names);

            var authorDictionary = new Dictionary<long, Author>();

            var author = await db.QueryAsync<Author>(query,
                parameters);

            return author.Distinct().ToList();
        }

        public async Task<List<Author>> GetAllAsync()
        {
            using (IDbConnection db = new SqlConnection(_options.DefaultConnection))
            {
                var query = @"SELECT *
                            FROM
                            Authors
                            ORDER BY Id";


                var authors = (await db.QueryAsync<Author>(query)).ToList();
                return authors;
            }
        }

        public async Task<Author> GetByIdAsync(long id)
        {
            var query = @"SELECT*
                        FROM
                        (SELECT*
                        FROM Authors WHERE Authors.Id = @Id)
                        AS author
                        LEFT JOIN AuthorPrintingEdition ON author.Id = AuthorPrintingEdition.AuthorsId
                        LEFT JOIN PrintingEditions ON AuthorPrintingEdition.PrintingEditionsId = PrintingEditions.Id";

            var parameters = new DynamicParameters();
            parameters.Add("@Id", id);
            return await GetAuthorFromDb(query, parameters);
        }

        public async Task<Author> GetByNameAsync(string name)
        {
                var query = @"SELECT*
                            FROM
                            (SELECT*
                            FROM Authors
                            WHERE Authors.Name = @Name)
                            AS author
                            LEFT JOIN AuthorPrintingEdition ON author.Id = AuthorPrintingEdition.AuthorsId
                            LEFT JOIN PrintingEditions ON AuthorPrintingEdition.PrintingEditionsId = PrintingEditions.Id";

                var parameters = new DynamicParameters();
                parameters.Add("@Name", name);
                return await GetAuthorFromDb(query, parameters);   
        }

        public async Task<bool> IsAuthorsInDbAsync(List<long> ids)
        {
            using (IDbConnection db = new SqlConnection(_options.DefaultConnection))
            {
                var query = @"SELECT COUNT(Authors.Id)
                            FROM Authors
                            WHERE Authors.Id IN @idList";

                var parameters = new DynamicParameters();
                parameters.Add("@idList", ids);

                int authorsCount = (await db.QueryAsync<int>(query, parameters)).FirstOrDefault();

                return authorsCount == ids.Count;
            }
        }
        public async Task CreateAsync(Author author)
        {

            using (IDbConnection db = new SqlConnection(_options.DefaultConnection))
            {
                var query = "INSERT INTO Authors(Name, DateOfCreation) VALUES(@Name, @Date); ";

                var parameters = new DynamicParameters();
                parameters.Add("@Name", author.Name);
                parameters.Add("@Date", author.DateOfCreation);

                await db.QueryAsync(query, parameters);
            }
        }

        public async Task UpdateAsync(Author author)
        {

            using (IDbConnection db = new SqlConnection(_options.DefaultConnection))
            {
                var query = "UPDATE Authors SET Name = @Name WHERE Authors.Id = @Id;";

                var parameters = new DynamicParameters();
                parameters.Add("@Name", author.Name);
                parameters.Add("@Id", author.Id);
                await db.QueryAsync(query, parameters);
            }
        }

        private async Task<Author> GetAuthorFromDb(string query, DynamicParameters parameters)
        {
            using (IDbConnection db = new SqlConnection(_options.DefaultConnection))
            {
                var authorDictionary = new Dictionary<long, Author>();

                var author =
                    (await db.QueryAsync<Author, PrintingEdition, Author>(query,
                    (author, editions) =>
                    {
                        Author authorEntry;
                        if (!authorDictionary.TryGetValue(author.Id, out authorEntry))
                        {
                            authorEntry = author;
                            authorEntry.PrintingEditions = new List<PrintingEdition>();
                            authorDictionary.Add(authorEntry.Id, authorEntry);
                        }
                        authorEntry.PrintingEditions.Add(editions);
                        return authorEntry;
                    },
                    parameters)).Distinct().FirstOrDefault();

                return author;
            }
        }
        public async Task DeleteAsync(long id)
        {
            using (IDbConnection db = new SqlConnection(_options.DefaultConnection))
            {
                await db.DeleteAsync<Author>(new Author { Id = id });
            }
        }
    }
}
