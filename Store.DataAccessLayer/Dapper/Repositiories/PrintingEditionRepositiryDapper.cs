using Dapper;
using Dapper.Contrib.Extensions;
using Microsoft.Extensions.Options;
using Store.DataAccessLayer.Dapper.HelperClasses;
using Store.DataAccessLayer.Dapper.Interfaces;
using Store.DataAccessLayer.Entities;
using Store.DataAccessLayer.FiltrationModels;
using Store.Sharing.Configuration;
using Store.Sharing.Constants;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Store.DataAccessLayer.Dapper.Repositiories
{
    public class PrintingEditionRepositiryDapper : IPrintingEditionRepositiryDapper
    {
        private readonly ConnectionStringConfig _options;
        public PrintingEditionRepositiryDapper(IOptions<ConnectionStringConfig> options)
        {
            _options = options.Value;
        }

        public async Task CreateAsync(PrintingEdition edition)
        {
            using (IDbConnection db = new SqlConnection(_options.DefaultConnection))
            {
                var queryAddEdition = @"INSERT INTO PrintingEditions(Title, DateOfCreation, Description, Status, Price, IsRemoved, Currency, EditionType)
                  VALUES (@Title, @Date, @Description, @Status, @Price, @IsRemoved, @Currency, @EditionType);
                  SELECT SCOPE_IDENTITY()";

                var editionParameters = new DynamicParameters();
                editionParameters.Add("@Title", edition.Title);
                editionParameters.Add("@Date", edition.DateOfCreation);
                editionParameters.Add("@Description", edition.Description);
                editionParameters.Add("@Status", edition.Status);
                editionParameters.Add("@Price", edition.Price);
                editionParameters.Add("@IsRemoved", edition.IsRemoved);
                editionParameters.Add("@Currency", edition.Currency);
                editionParameters.Add("@EditionType", edition.EditionType);

                long editionId = (await db.QueryAsync<long>(queryAddEdition, editionParameters)).FirstOrDefault();

                List<long> authorsIds = edition.Authors.Select(id => id.Id).ToList();

                List<AuthorIdEditionId> authorsIdsEditionsIds = new();

                foreach (var authorId in authorsIds)
                {
                    AuthorIdEditionId authorIdEditionId = new(authorId, editionId);
                    authorsIdsEditionsIds.Add(authorIdEditionId);
                }

                string queryAddAuthorEditon = "INSERT INTO AuthorPrintingEdition VALUES (@AuthorId, @EditionId)";
                db.Execute(queryAddAuthorEditon, authorsIdsEditionsIds);
            }
        }

        public async Task<(IEnumerable<PrintingEdition>, int, double, double)> GetAsync(EditionFiltrationModelDAL model)
        {
            var skip = (model.CurrentPage - Constants.PaginationParams.DEFAULT_OFFSET) * model.PageSize;
            string sortDirection = model.IsAscending ? Constants.SortingParams.SORT_ASC : Constants.SortingParams.SORT_DESC;

            using (IDbConnection db = new SqlConnection(_options.DefaultConnection))
            {
                string queryGetEdition =
                @"IF @propertyForSort = 'Id' AND @sortDirection = 'ASC'
                SELECT*
                FROM(
                SELECT *
                FROM PrintingEditions WHERE(@id is null OR PrintingEditions.Id = @id)
                AND PrintingEditions.Title LIKE @title
                AND(@currency is null OR PrintingEditions.Currency = @currency)
                AND(@minPrice is null OR PrintingEditions.Price >= @minPrice)
                AND(@maxPrice is null OR PrintingEditions.Price <= @maxPrice)
                AND PrintingEditions.EditionType IN @editionType
                ORDER BY Id ASC 
                OFFSET @skip ROWS FETCH NEXT @pageSize ROWS ONLY 
                ) AS edition 
                JOIN AuthorPrintingEdition ON edition.Id = AuthorPrintingEdition.PrintingEditionsId
                JOIN Authors ON AuthorPrintingEdition.AuthorsId = Authors.Id
                ORDER BY edition.Id ASC

                IF @propertyForSort = 'Id' AND @sortDirection = 'DESC'
                SELECT*
                FROM(
                SELECT *
                FROM PrintingEditions WHERE(@id is null OR PrintingEditions.Id = @id)
                AND PrintingEditions.Title LIKE @title
                AND(@currency is null OR PrintingEditions.Currency = @currency)
                AND(@minPrice is null OR PrintingEditions.Price >= @minPrice)
                AND(@maxPrice is null OR PrintingEditions.Price <= @maxPrice)
                AND PrintingEditions.EditionType IN @editionType
                ORDER BY Id DESC 
                OFFSET @skip ROWS FETCH NEXT @pageSize ROWS ONLY 
                ) AS edition 
                JOIN AuthorPrintingEdition ON edition.Id = AuthorPrintingEdition.PrintingEditionsId
                JOIN Authors ON AuthorPrintingEdition.AuthorsId = Authors.Id
                ORDER BY edition.Id DESC


                IF @propertyForSort = 'Title' AND @sortDirection = 'ASC'
                SELECT*
                FROM(
                SELECT *
                FROM PrintingEditions WHERE(@id is null OR PrintingEditions.Id = @id)
                AND PrintingEditions.Title LIKE @title
                AND(@currency is null OR PrintingEditions.Currency = @currency)
                AND(@minPrice is null OR PrintingEditions.Price >= @minPrice)
                AND(@maxPrice is null OR PrintingEditions.Price <= @maxPrice)
                AND PrintingEditions.EditionType IN @editionType
                ORDER BY Title ASC 
                OFFSET @skip ROWS FETCH NEXT @pageSize ROWS ONLY 
                ) AS edition 
                JOIN AuthorPrintingEdition ON edition.Id = AuthorPrintingEdition.PrintingEditionsId
                JOIN Authors ON AuthorPrintingEdition.AuthorsId = Authors.Id
                ORDER BY edition.Title ASC

                IF @propertyForSort = 'Title' AND @sortDirection = 'DESC'
                SELECT*
                FROM(
                SELECT *
                FROM PrintingEditions WHERE(@id is null OR PrintingEditions.Id = @id)
                AND PrintingEditions.Title LIKE @title
                AND(@currency is null OR PrintingEditions.Currency = @currency)
                AND(@minPrice is null OR PrintingEditions.Price >= @minPrice)
                AND(@maxPrice is null OR PrintingEditions.Price <= @maxPrice)
                AND PrintingEditions.EditionType IN @editionType
                ORDER BY Title DESC 
                OFFSET @skip ROWS FETCH NEXT @pageSize ROWS ONLY 
                ) AS edition 
                JOIN AuthorPrintingEdition ON edition.Id = AuthorPrintingEdition.PrintingEditionsId
                JOIN Authors ON AuthorPrintingEdition.AuthorsId = Authors.Id
                ORDER BY edition.Title DESC


                IF @propertyForSort = 'Price' AND @sortDirection = 'ASC'
                SELECT*
                FROM(
                SELECT *
                FROM PrintingEditions WHERE(@id is null OR PrintingEditions.Id = @id)
                AND PrintingEditions.Title LIKE @title
                AND(@currency is null OR PrintingEditions.Currency = @currency)
                AND(@minPrice is null OR PrintingEditions.Price >= @minPrice)
                AND(@maxPrice is null OR PrintingEditions.Price <= @maxPrice)
                AND PrintingEditions.EditionType IN @editionType
                ORDER BY Price ASC 
                OFFSET @skip ROWS FETCH NEXT @pageSize ROWS ONLY 
                ) AS edition 
                JOIN AuthorPrintingEdition ON edition.Id = AuthorPrintingEdition.PrintingEditionsId
                JOIN Authors ON AuthorPrintingEdition.AuthorsId = Authors.Id
                ORDER BY edition.Price ASC

                IF @propertyForSort = 'Price' AND @sortDirection = 'DESC'
                SELECT*
                FROM(
                SELECT *
                FROM PrintingEditions WHERE(@id is null OR PrintingEditions.Id = @id)
                AND PrintingEditions.Title LIKE @title
                AND(@currency is null OR PrintingEditions.Currency = @currency)
                AND(@minPrice is null OR PrintingEditions.Price >= @minPrice)
                AND(@maxPrice is null OR PrintingEditions.Price <= @maxPrice)
                AND PrintingEditions.EditionType IN @editionType
                ORDER BY Price DESC
                OFFSET @skip ROWS FETCH NEXT @pageSize ROWS ONLY
                ) AS edition
                JOIN AuthorPrintingEdition ON edition.Id = AuthorPrintingEdition.PrintingEditionsId
                JOIN Authors ON AuthorPrintingEdition.AuthorsId = Authors.Id
                ORDER BY edition.Price DESC


                IF @propertyForSort = 'Currency' AND @sortDirection = 'ASC'
                SELECT*
                FROM(
                SELECT *
                FROM PrintingEditions WHERE(@id is null OR PrintingEditions.Id = @id)
                AND PrintingEditions.Title LIKE @title
                AND(@currency is null OR PrintingEditions.Currency = @currency)
                AND(@minPrice is null OR PrintingEditions.Price >= @minPrice)
                AND(@maxPrice is null OR PrintingEditions.Price <= @maxPrice)
                AND PrintingEditions.EditionType IN @editionType
                ORDER BY Currency ASC 
                OFFSET @skip ROWS FETCH NEXT @pageSize ROWS ONLY 
                ) AS edition 
                JOIN AuthorPrintingEdition ON edition.Id = AuthorPrintingEdition.PrintingEditionsId
                JOIN Authors ON AuthorPrintingEdition.AuthorsId = Authors.Id
                ORDER BY edition.Currency ASC

                IF @propertyForSort = 'Currency' AND @sortDirection = 'DESC'
                SELECT*
                FROM(
                SELECT *
                FROM PrintingEditions WHERE(@id is null OR PrintingEditions.Id = @id)
                AND PrintingEditions.Title LIKE @title
                AND(@currency is null OR PrintingEditions.Currency = @currency)
                AND(@minPrice is null OR PrintingEditions.Price >= @minPrice)
                AND(@maxPrice is null OR PrintingEditions.Price <= @maxPrice)
                AND PrintingEditions.EditionType IN @editionType
                ORDER BY Currency DESC 
                OFFSET @skip ROWS FETCH NEXT @pageSize ROWS ONLY 
                ) AS edition 
                JOIN AuthorPrintingEdition ON edition.Id = AuthorPrintingEdition.PrintingEditionsId
                JOIN Authors ON AuthorPrintingEdition.AuthorsId = Authors.Id
                ORDER BY edition.Currency DESC


                IF @propertyForSort = 'Status' AND @sortDirection = 'ASC'
                SELECT*
                FROM(
                SELECT *
                FROM PrintingEditions WHERE(@id is null OR PrintingEditions.Id = @id)
                AND PrintingEditions.Title LIKE @title
                AND(@currency is null OR PrintingEditions.Currency = @currency)
                AND(@minPrice is null OR PrintingEditions.Price >= @minPrice)
                AND(@maxPrice is null OR PrintingEditions.Price <= @maxPrice)
                AND PrintingEditions.EditionType IN @editionType
                ORDER BY Status ASC 
                OFFSET @skip ROWS FETCH NEXT @pageSize ROWS ONLY 
                ) AS edition 
                JOIN AuthorPrintingEdition ON edition.Id = AuthorPrintingEdition.PrintingEditionsId
                JOIN Authors ON AuthorPrintingEdition.AuthorsId = Authors.Id
                ORDER BY edition.Status ASC

                IF @propertyForSort = 'Status' AND @sortDirection = 'DESC'
                SELECT*
                FROM(
                SELECT *
                FROM PrintingEditions WHERE(@id is null OR PrintingEditions.Id = @id)
                AND PrintingEditions.Title LIKE @title
                AND(@currency is null OR PrintingEditions.Currency = @currency)
                AND(@minPrice is null OR PrintingEditions.Price >= @minPrice)
                AND(@maxPrice is null OR PrintingEditions.Price <= @maxPrice)
                AND PrintingEditions.EditionType IN @editionType
                ORDER BY Status DESC 
                OFFSET @skip ROWS FETCH NEXT @pageSize ROWS ONLY 
                ) AS edition 
                JOIN AuthorPrintingEdition ON edition.Id = AuthorPrintingEdition.PrintingEditionsId
                JOIN Authors ON AuthorPrintingEdition.AuthorsId = Authors.Id
                ORDER BY edition.Status DESC";

                var parameters = new DynamicParameters();
                parameters.Add("@propertyForSort", model.PropertyForSort);
                parameters.Add("@skip", skip);
                parameters.Add("@pageSize", model.PageSize);
                parameters.Add("@title", $"%{model.Title}%");
                parameters.Add("@id", model.Id);
                parameters.Add("@currency", model.Currency);
                parameters.Add("@sortDirection", sortDirection);
                parameters.Add("@minPrice", model.MinPrice);
                parameters.Add("@maxPrice", model.MaxPrice);
                parameters.Add("@editionType", model.EditionType);

                var editionDictionary = new Dictionary<long, PrintingEdition>();

                var editions =
                    (await db.QueryAsync<PrintingEdition, Author, PrintingEdition>(queryGetEdition,
                    (edition, author) =>
                    {
                        PrintingEdition editionEntry;
                        if (!editionDictionary.TryGetValue(edition.Id, out editionEntry))
                        {
                            editionEntry = edition;
                            editionEntry.Authors = new List<Author>();
                            editionDictionary.Add(editionEntry.Id, editionEntry);
                        }
                        editionEntry.Authors.Add(author);
                        return editionEntry;
                    },
                    parameters)).Distinct().ToList();

                int count = default;
                double minPrice = model.CurrentSliderFlor;
                double maxPrice = model.CurrentSliderCeil;
                var result = (editions: editions, count: count, minPrice: minPrice, maxPrice: maxPrice);

                if (!editions.Any())
                {
                    return result;
                }

                string queryGetCount = @"SELECT COUNT (PrintingEditions.Id) 
                FROM PrintingEditions 
                WHERE (@id is null OR PrintingEditions.Id = @id)
                AND PrintingEditions.Title LIKE @title
                AND (@currency is null OR PrintingEditions.Currency = @currency)
                AND (@minPrice is null OR PrintingEditions.Price >= @minPrice)
                AND (@maxPrice is null OR PrintingEditions.Price <= @maxPrice) 
                AND PrintingEditions.EditionType IN @editionType";

                count = (await db.QueryAsync<int>(queryGetCount, parameters)).FirstOrDefault();

                string queryGetMaxPrice = @"SELECT MAX (PrintingEditions.Price) 
                FROM PrintingEditions 
                WHERE (@id is null OR PrintingEditions.Id = @id)
                AND PrintingEditions.Title LIKE @title
                AND (@currency is null OR PrintingEditions.Currency = @currency) 
                AND PrintingEditions.EditionType IN @editionType";

                maxPrice = (await db.QueryAsync<int>(queryGetMaxPrice, parameters)).FirstOrDefault();

                string queryGetMinPrice = @"SELECT MIN (PrintingEditions.Price) 
                FROM PrintingEditions 
                WHERE (@id is null OR PrintingEditions.Id = @id)
                AND PrintingEditions.Title LIKE @title
                AND (@currency is null OR PrintingEditions.Currency = @currency)
                AND PrintingEditions.EditionType IN @editionType";

                minPrice = (await db.QueryAsync<int>(queryGetMinPrice, parameters)).FirstOrDefault();

                result = (editions: editions, count: count, minPrice: minPrice, maxPrice: maxPrice);
                return result;
            }
        }

        public async Task<PrintingEdition> GetByIdAsync(long id)
        {
            var query = @"SELECT*
                        FROM(
                        SELECT *
                        FROM PrintingEditions
                        WHERE PrintingEditions.Id = @Id)
                        AS edition
                        JOIN AuthorPrintingEdition ON edition.Id = AuthorPrintingEdition.PrintingEditionsId
                        JOIN Authors ON AuthorPrintingEdition.AuthorsId = Authors.Id";

            var parameters = new DynamicParameters();
            parameters.Add("@Id", id);
            return await GetEditionFromDb(query, parameters);
        }

        public async Task<PrintingEdition> GetByTitleAsync(string title)
        {
            var query = @"SELECT* 
            FROM(
            SELECT *
            FROM PrintingEditions
            WHERE PrintingEditions.Title = @Title
            ) AS edition	 
            JOIN AuthorPrintingEdition ON edition.Id = AuthorPrintingEdition.PrintingEditionsId
            JOIN Authors ON AuthorPrintingEdition.AuthorsId = Authors.Id";

            var parameters = new DynamicParameters();
            parameters.Add("@Title", title);
            return await GetEditionFromDb(query, parameters);
        }

        public async Task UpdateAsync(PrintingEdition edition)
        {

            using (IDbConnection db = new SqlConnection(_options.DefaultConnection))
            {
                var queryUpdateEdition = @"UPDATE PrintingEditions 
                SET Title = @Title, 
                Description = @Description, 
                Price = @Price,
                EditionType = @EditionType,
                Currency = @Currency,
                Status = @Status
                WHERE PrintingEditions.Id = @Id

                DELETE FROM AuthorPrintingEdition WHERE PrintingEditionsId=@Id;";

                var parameters = new DynamicParameters();
                parameters.Add("@Title", edition.Title);
                parameters.Add("@Description", edition.Description  );
                parameters.Add("@Price", edition.Price);
                parameters.Add("@EditionType", edition.EditionType);
                parameters.Add("@Currency", edition.Currency);
                parameters.Add("@Status", edition.Status);
                parameters.Add("@Id", edition.Id);

                await db.QueryAsync(queryUpdateEdition, parameters);

                List<long> authorsIds = edition.Authors.Select(id => id.Id).ToList();

                List<AuthorIdEditionId> authorsIdsEditionsIds = new();

                foreach (var authorId in authorsIds)
                {
                    AuthorIdEditionId authorIdEditionId = new(authorId, edition.Id);
                    authorsIdsEditionsIds.Add(authorIdEditionId);
                }

                var queryAddAuthorEditon = "INSERT INTO AuthorPrintingEdition VALUES (@AuthorId, @EditionId)";
                db.Execute(queryAddAuthorEditon, authorsIdsEditionsIds);
            }
        }

        public async Task<List<PrintingEdition>> GetEditionsListByIdListAsync(List<long> ids)
        {
            using (IDbConnection db = new SqlConnection(_options.DefaultConnection))
            { 
               string queryGetEditions = @"SELECT*
                FROM PrintingEditions WHERE PrintingEditions.Id IN @ids
                ORDER BY Id ASC";

                var parameters = new DynamicParameters();
                parameters.Add("@ids", ids);

                List<PrintingEdition> editions = (await db.QueryAsync<PrintingEdition>(queryGetEditions, parameters)).ToList();
                return editions;
            } 
        }

        private async Task<PrintingEdition> GetEditionFromDb(string query, DynamicParameters parameters)
        {
            using (IDbConnection db = new SqlConnection(_options.DefaultConnection))
            {
                var editionDictionary = new Dictionary<long, PrintingEdition>();

                var edition =
                    (await db.QueryAsync<PrintingEdition, Author, PrintingEdition>(query,
                    (author, editions) =>
                    {
                        PrintingEdition editionEntry;
                        if (!editionDictionary.TryGetValue(author.Id, out editionEntry))
                        {
                            editionEntry = author;
                            editionEntry.Authors = new List<Author>();
                            editionDictionary.Add(editionEntry.Id, editionEntry);
                        }
                        editionEntry.Authors.Add(editions);
                        return editionEntry;
                    },
                    parameters)).Distinct().FirstOrDefault();

                return edition;
            }
        }

        public async Task DeleteAsync(long id)
        {
            using (IDbConnection db = new SqlConnection(_options.DefaultConnection))
            {
                await db.DeleteAsync<PrintingEdition>(new PrintingEdition {Id = id});  
            }
        }
    }
}
