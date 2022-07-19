using Store.BusinessLogicLayer.Models.Base;
using Store.Sharing.Constants;
using static Store.DataAccessLayer.Enums.Enums;

namespace Store.BusinessLogicLayer.Models.Orders
{
    public class OrderFiltrationModel : BaseFiltrationModel
    {
        public string Description { get; set; }//TODO wrong spelling+++
        public long? UserId { get; set; }
        public OrderStatusState? Status { get; set; }
        public OrderFiltrationModel()
        {
            PropertyForSort = Constants.SortingParams.ORDER_DEF_SORT_PROP;
        }
    }
} 
