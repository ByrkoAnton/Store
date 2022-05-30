using Dapper;
using Microsoft.Extensions.Options;
using Store.DataAccessLayer.Dapper.Interfaces;
using Store.DataAccessLayer.Entities;
using Store.DataAccessLayer.FiltrationModels;
using Store.Sharing.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
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
        public Task<(IEnumerable<PrintingEdition>, int, double, double)> GetAsync(EditionFiltrationModelDAL model)
        {
            throw new NotImplementedException();
        }

        public async Task<PrintingEdition> GetByIdAsync(long id)
        {
            var query = "SELECT* FROM(SELECT * FROM PrintingEditions WHERE PrintingEditions.Id = @Id) AS edition JOIN AuthorPrintingEdition ON edition.Id = AuthorPrintingEdition.PrintingEditionsId JOIN Authors ON AuthorPrintingEdition.AuthorsId = Authors.Id";

            var parameters = new DynamicParameters();
            parameters.Add("@Id", id);
            return await GetEditionFromDb(query, parameters);
        }

        public Task<PrintingEdition> GetByTitle(string description)
        {
            throw new NotImplementedException();
        }

        public Task<List<PrintingEdition>> GetEditionsListByIdListAsync(List<long> id)
        {
            throw new NotImplementedException();
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
    }
}
