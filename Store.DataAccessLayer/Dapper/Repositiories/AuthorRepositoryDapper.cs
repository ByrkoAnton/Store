using Dapper;
using Dapper.Contrib.Extensions;
using Microsoft.Extensions.Options;
using Store.BusinessLogicLayer.Configuration;
using Store.DataAccessLayer.Dapper.Interfaces;
using Store.DataAccessLayer.Entities;
using Store.DataAccessLayer.Models.FiltrationModels;
using Store.Sharing.Configuration;
using Store.Sharing.Constants;
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
        public async Task<(IEnumerable<Author>, int)> GetAsync(AuthorFiltrationModelDAL model)
        {
            var skip = (model.CurrentPage - Constants.PaginationParams.DEFAULT_OFFSET) * model.PageSize;

            using (IDbConnection db = new SqlConnection(_options.DefaultConnection))
            {
                //var queryGetAuthors = $"SELECT author.Id, author.DateOfCreation, author.Name, editionAuthorPrintingEdition.Id, editionAuthorPrintingEdition.Currency, editionAuthorPrintingEdition.DateOfCreation, editionAuthorPrintingEdition.Description, editionAuthorPrintingEdition.EditionType, editionAuthorPrintingEdition.IsRemoved, editionAuthorPrintingEdition.Price, editionAuthorPrintingEdition.Title FROM(SELECT Id, DateOfCreation, Name FROM Authors WHERE Name LIKE N'%{model.Name}%' ORDER BY {model.PropertyForSort} OFFSET {skip} ROWS FETCH NEXT {model.PageSize} ROWS ONLY) AS author LEFT JOIN(SELECT edition.Id, edition.Currency, edition.DateOfCreation, edition.Description, edition.EditionType, edition.IsRemoved, edition.Price, edition.Status, edition.Title, AuthorPrintingEdition.AuthorsId, AuthorPrintingEdition.PrintingEditionsId FROM AuthorPrintingEdition INNER JOIN PrintingEditions AS edition ON AuthorPrintingEdition.PrintingEditionsId = edition.Id) AS editionAuthorPrintingEdition ON author.Id = editionAuthorPrintingEdition.AuthorsId ORDER BY author.{model.PropertyForSort}";

                //var queryGetAuthors = $"SET @sqlText = N'SELECT author.Id, author.DateOfCreation, author.Name, editionAuthorPrintingEdition.Id, editionAuthorPrintingEdition.Currency, editionAuthorPrintingEdition.DateOfCreation, editionAuthorPrintingEdition.Description, editionAuthorPrintingEdition.EditionType, editionAuthorPrintingEdition.IsRemoved, editionAuthorPrintingEdition.Price, editionAuthorPrintingEdition.Title FROM(SELECT Id, DateOfCreation, Name FROM Authors ORDER BY {model.PropertyForSort} OFFSET ' + @skip + ' ROWS FETCH NEXT @pageSize ROWS ONLY) AS author LEFT JOIN(SELECT edition.Id, edition.Currency, edition.DateOfCreation, edition.Description, edition.EditionType, edition.IsRemoved, edition.Price, edition.Status, edition.Title,AuthorPrintingEdition.AuthorsId, AuthorPrintingEdition.PrintingEditionsId FROM AuthorPrintingEdition INNER JOIN PrintingEditions AS edition ON AuthorPrintingEdition.PrintingEditionsId = edition.Id) AS editionAuthorPrintingEdition ON author.Id = editionAuthorPrintingEdition.AuthorsId ORDER BY author.{model.PropertyForSort}' Exec(@sqlText)";

                int sortParam = model.PropertyForSort == "Id" ? 1 : 2;

                string sortDirection = model.IsAscending ? "ASC" : "DESC";
                var queryGetAuthors = "IF @propertyForSort = 'Name' AND @sortDirection = 'ASC' SELECT* FROM( SELECT * FROM Authors WHERE Authors.Name LIKE @nameForSearch ORDER BY Name ASC OFFSET @skip ROWS FETCH NEXT @pageSize ROWS ONLY ) AS author LEFT JOIN AuthorPrintingEdition ON author.Id = AuthorPrintingEdition.AuthorsId LEFT JOIN PrintingEditions ON AuthorPrintingEdition.PrintingEditionsId = PrintingEditions.Id ORDER BY author.Name ASC IF @propertyForSort = 'Name' AND @sortDirection = 'DESC' SELECT* FROM( SELECT * FROM Authors WHERE Authors.Name LIKE @nameForSearch ORDER BY Name DESC OFFSET @skip ROWS FETCH NEXT @pageSize ROWS ONLY) AS author LEFT JOIN AuthorPrintingEdition ON author.Id = AuthorPrintingEdition.AuthorsId LEFT JOIN PrintingEditions ON AuthorPrintingEdition.PrintingEditionsId = PrintingEditions.Id ORDER BY author.Name DESC IF @propertyForSort = 'Id' AND @sortDirection = 'ASC' SELECT* FROM( SELECT * FROM Authors WHERE Authors.Name LIKE @nameForSearch ORDER BY Id ASC OFFSET @skip ROWS FETCH NEXT @pageSize ROWS ONLY) AS author LEFT JOIN AuthorPrintingEdition ON author.Id = AuthorPrintingEdition.AuthorsId LEFT JOIN PrintingEditions ON AuthorPrintingEdition.PrintingEditionsId = PrintingEditions.Id ORDER BY author.Id ASC IF @propertyForSort = 'Id' AND @sortDirection = 'DESC' SELECT* FROM( SELECT * FROM Authors WHERE Authors.Name LIKE @nameForSearch ORDER BY Id DESC OFFSET @skip ROWS FETCH NEXT @pageSize ROWS ONLY) AS author LEFT JOIN AuthorPrintingEdition ON author.Id = AuthorPrintingEdition.AuthorsId LEFT JOIN PrintingEditions ON AuthorPrintingEdition.PrintingEditionsId = PrintingEditions.Id ORDER BY author.Id DESC";

                var parameters = new DynamicParameters();
                parameters.Add("@propertyForSort", model.PropertyForSort);
                parameters.Add("@skip", skip);
                parameters.Add("@pageSize", model.PageSize);
                parameters.Add("@nameForSearch", $"%{model.Name}%");
                parameters.Add("@sortDirection", sortDirection);


                

                var authorDictionary = new Dictionary<long, Author>();

                var q = db.Query(queryGetAuthors, parameters);
                var authors =
                    await db.QueryAsync<Author, PrintingEdition, Author>(queryGetAuthors,
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
                    parameters);

                //.Distinct()
                //.ToList();

                authors = authors.Distinct().ToList();

                var queryGetAuthorsCount = $"SELECT Authors.Id FROM Authors WHERE Name LIKE N'%{model.Name}%'";
                var authorsCount = db.Query(queryGetAuthorsCount).Count();

                var authorsWithCount = (authors: authors, count: authorsCount);
                return authorsWithCount;
            }
        }

        public async Task<List<Author>> GetAuthorsListByNamesListAsync(List<string> names)
        {
            for (int i = 0; i < names.Count; i++)
            {
                names[i] = $"'{names[i]}'";
            }
            string authorsNames = String.Join(',', names);

            using (IDbConnection db = new SqlConnection(_options.DefaultConnection))
            {
                var query = $"SELECT Authors.Id, Authors.Name, Authors.DateOfCreation, t.Id, t.Currency, t.DateOfCreation, t.Description, t.EditionType, t.Price, t.Status,t.IsRemoved, t.Title, t.AuthorsId, t.PrintingEditionsId FROM Authors LEFT JOIN (SELECT PrintingEditions.Id, PrintingEditions.Currency, PrintingEditions.DateOfCreation, PrintingEditions.Description, PrintingEditions.EditionType, PrintingEditions.Price, PrintingEditions.Status, PrintingEditions.IsRemoved, PrintingEditions.Title, AuthorPrintingEdition.AuthorsId, AuthorPrintingEdition.PrintingEditionsId FROM AuthorPrintingEdition INNER JOIN PrintingEditions ON AuthorPrintingEdition.PrintingEditionsId = PrintingEditions.Id) AS[t] ON Authors.Id = t.AuthorsId WHERE Authors.Name in ({authorsNames})";

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

                return author.Distinct().ToList();
            }
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
