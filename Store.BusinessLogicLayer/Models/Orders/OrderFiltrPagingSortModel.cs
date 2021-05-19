using Store.BusinessLogicLayer.Models.Base;
using static Store.DataAccessLayer.Enums.Enums;
namespace Store.BusinessLogicLayer.Models.Orders
{
    public class OrderFiltrPagingSortModel : BaseFiltrPaginSortModel
    {
        public string Discription { get; set; }
        public long? UserId { get; set; }
        public OrderStatus? Status { get; set; }
        public long? EditionId { get; set; }

        public OrderFiltrPagingSortModel()
        {
            PropertyForSort = "id";//const
        }
    }
} 
