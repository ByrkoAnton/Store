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

namespace Store.DataAccessLayer.Dapper.Repositiories//TODO wrong selling
{
    public class OrderRepositoryDapper : IOrderRepositoryDapper
    {
        private readonly ConnectionStringConfig _options;
        public OrderRepositoryDapper(IOptions<ConnectionStringConfig> options)
        {
            _options = options.Value;
        }

        public async Task CreateAsync(Order order)
        {
            using IDbConnection db = new SqlConnection(_options.DefaultConnection);
            await db.InsertAsync<Order>(order);//TODO redundant specification
        }
        public async Task<(IEnumerable<Order>, int)> GetAsync(OrderFiltrationModelDAL model)
        {
            var skip = (model.CurrentPage - Constants.PaginationParams.DEFAULT_OFFSET) * model.PageSize;
            string sortDirection = model.IsAscending ? Constants.SortingParams.SORT_ASC : Constants.SortingParams.SORT_DESC;

            using IDbConnection db = new SqlConnection(_options.DefaultConnection);
            string queryGetOrders = //TODO wrong spelling 'description'
                                    //TODO please check dapper extention and sorting https://riptutorial.com/dapper-with-mvc/learn/100006/sorting
            @"IF @propertyForSort = 'Id' AND @sortDirection = 'ASC'
                SELECT* 
                FROM Orders 
                WHERE (@userId is null OR Orders.UserId = @userId)
                AND (@discription is null OR Orders.Discription Like @discription)
                ORDER BY Id ASC
                OFFSET @skip ROWS FETCH NEXT @pageSize ROWS ONLY

                IF @propertyForSort = 'Id' AND @sortDirection = 'DESC'
                SELECT* 
                FROM Orders 
                WHERE (@userId is null OR Orders.UserId = @userId)
                AND (@discription is null OR Orders.Discription Like @discription)
                ORDER BY Id DESC
                OFFSET @skip ROWS FETCH NEXT @pageSize ROWS ONLY


                IF @propertyForSort = 'DateOfCreation' AND @sortDirection = 'ASC'
                SELECT* 
                FROM Orders 
                WHERE (@userId is null OR Orders.UserId = @userId)
                AND (@discription is null OR Orders.Discription Like @discription)
                ORDER BY DateOfCreation ASC
                OFFSET @skip ROWS FETCH NEXT @pageSize ROWS ONLY

                IF @propertyForSort = 'DateOfCreation' AND @sortDirection = 'DESC'
                SELECT* 
                FROM Orders 
                WHERE (@userId is null OR Orders.UserId = @userId)
                AND (@discription is null OR Orders.Discription Like @discription)
                ORDER BY DateOfCreation DESC
                OFFSET @skip ROWS FETCH NEXT @pageSize ROWS ONLY


                IF @propertyForSort = 'UserId' AND @sortDirection = 'ASC'
                SELECT* 
                FROM Orders 
                WHERE (@userId is null OR Orders.UserId = @userId)
                AND (@discription is null OR Orders.Discription Like @discription)
                ORDER BY UserId ASC
                OFFSET @skip ROWS FETCH NEXT @pageSize ROWS ONLY

                IF @propertyForSort = 'UserId' AND @sortDirection = 'DESC'
                SELECT* 
                FROM Orders 
                WHERE (@userId is null OR Orders.UserId = @userId)
                AND (@discription is null OR Orders.Discription Like @discription)
                ORDER BY UserId DESC
                OFFSET @skip ROWS FETCH NEXT @pageSize ROWS ONLY


                IF @propertyForSort = 'PaymentId' AND @sortDirection = 'ASC'
                SELECT* 
                FROM Orders 
                WHERE (@userId is null OR Orders.UserId = @userId)
                AND (@discription is null OR Orders.Discription Like @discription)
                ORDER BY PaymentId ASC
                OFFSET @skip ROWS FETCH NEXT @pageSize ROWS ONLY

                IF @propertyForSort = 'PaymentId' AND @sortDirection = 'DESC'
                SELECT* 
                FROM Orders 
                WHERE (@userId is null OR Orders.UserId = @userId)
                AND (@discription is null OR Orders.Discription Like @discription)
                ORDER BY PaymentId DESC
                OFFSET @skip ROWS FETCH NEXT @pageSize ROWS ONLY


                IF @propertyForSort = 'Status' AND @sortDirection = 'ASC'
                SELECT* 
                FROM Orders 
                WHERE (@userId is null OR Orders.UserId = @userId)
                AND (@discription is null OR Orders.Discription Like @discription)
                ORDER BY Status ASC
                OFFSET @skip ROWS FETCH NEXT @pageSize ROWS ONLY
                
                IF @propertyForSort = 'Status' AND @sortDirection = 'DESC'
                SELECT* 
                FROM Orders 
                WHERE (@userId is null OR Orders.UserId = @userId)
                AND (@discription is null OR Orders.Discription Like @discription)
                ORDER BY Status DESC
                OFFSET @skip ROWS FETCH NEXT @pageSize ROWS ONLY";

            var parameters = new DynamicParameters();
            parameters.Add("@propertyForSort", model.PropertyForSort);
            parameters.Add("@skip", skip);
            parameters.Add("@pageSize", model.PageSize);
            parameters.Add("@discription", string.IsNullOrWhiteSpace(model.Discription) ? null : $"%{model.Discription}%");
            parameters.Add("@userId", model.UserId);
            parameters.Add("@sortDirection", sortDirection);

            List<Order> orders = (await db.QueryAsync<Order>(queryGetOrders, parameters)).ToList();

            string queryGetCount = @"SELECT COUNT (Orders.Id)
                FROM Orders
                WHERE(@Userid is null OR Orders.UserId = @userId)
                AND(@discription is null OR Orders.Discription Like @discription)";

            int count = (await db.QueryAsync<int>(queryGetCount, parameters)).FirstOrDefault();
            var ordersWithCount = (orders: orders, count: count);
            return ordersWithCount;
        }

        public async Task<Order> GetByIdAsync(long id)
        {
            var query = @"SELECT*    
                        FROM Orders 
                        JOIN OrderItems ON OrderItems.OrderId = Orders.Id
                        WHERE Orders.Id = @id"; 

            using IDbConnection db = new SqlConnection(_options.DefaultConnection);
            var orderDictionary = new Dictionary<long, Order>();

            var parameters = new DynamicParameters();
            parameters.Add("@Id", id);

            var result =
                (await db.QueryAsync<Order, OrderItem, Order>(query,
                (order, orderItem) =>
                {
                    Order orderEntry;
                    if (!orderDictionary.TryGetValue(order.Id, out orderEntry))
                    {
                        orderEntry = order;
                        orderEntry.OrderItems = new List<OrderItem>();
                        orderDictionary.Add(orderEntry.Id, orderEntry);
                    }
                    orderEntry.OrderItems.Add(orderItem);
                    return orderEntry;
                },
                parameters)).Distinct().FirstOrDefault();

            return result;
        }

        public async Task UpdateAsync(Order order)
        {
            using IDbConnection db = new SqlConnection(_options.DefaultConnection);
            await db.UpdateAsync<Order>(order);
        } 
        public async Task DeleteAsync(long id)
        {
            using IDbConnection db = new SqlConnection(_options.DefaultConnection);
            await db.DeleteAsync<Order>(new Order { Id = id });
        }
    }
}
