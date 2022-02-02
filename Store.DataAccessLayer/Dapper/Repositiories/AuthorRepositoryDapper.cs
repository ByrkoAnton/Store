using Dapper;
using Dapper.Contrib.Extensions;
using Microsoft.Extensions.Options;
using Store.BusinessLogicLayer.Configuration;
using Store.DataAccessLayer.Dapper.Interfaces;
using Store.DataAccessLayer.Entities;
using Store.DataAccessLayer.Models.FiltrationModels;
using Store.Sharing.Configuration;
using System;
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
        public Task<(IEnumerable<Author>, int)> GetAsync(AuthorFiltrationModelDAL model)
        {
            throw new NotImplementedException();
        }

        public Task<List<Author>> GetAuthorsListByNamesListAsync(List<string> names)
        {
            throw new NotImplementedException();
        }

        public async Task<Author> GetByIdAsync(long id)
        {
            //using (var connection = new SqlConnection(_options.DefaultConnection))
            //{
            //    connection.Open();
            //    var invoice = connection.Get<Author>(id);
            //    return invoice;
            //}

            using (IDbConnection db = new SqlConnection(_options.DefaultConnection))
            {
                var query = $"SELECT Authors.Id, Authors.Name, Authors.DateOfCreation, t.Id, t.Currency, t.DateOfCreation, t.Description, t.EditionType, t.Price, t.Status,t.IsRemoved, t.Title, t.AuthorsId, t.PrintingEditionsId FROM Authors LEFT JOIN(SELECT PrintingEditions.Id, PrintingEditions.Currency, PrintingEditions.DateOfCreation, PrintingEditions.Description, PrintingEditions.EditionType, PrintingEditions.Price, PrintingEditions.Status,PrintingEditions.IsRemoved, PrintingEditions.Title, AuthorPrintingEdition.AuthorsId, AuthorPrintingEdition.PrintingEditionsId FROM AuthorPrintingEdition INNER JOIN  PrintingEditions ON AuthorPrintingEdition.PrintingEditionsId = PrintingEditions.Id)AS[t] ON Authors.Id = t.AuthorsId WHERE Authors.Id = {id}";

                var authorDictionary = new Dictionary<long, Author>();

                var author = await db.QueryAsync<Author, PrintingEdition, Author>(query,
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
                    splitOn: "Id");
            
                return author.Distinct().FirstOrDefault();
            }
        }

        public Task<Author> GetByNameAsync(string name)
        {
            throw new NotImplementedException();
        }

        public bool IsAuthorsInDb(List<long> id)
        {
            throw new NotImplementedException();
        }
    }
}
