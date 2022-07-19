using static Store.DataAccessLayer.Enums.Enums;

namespace Store.DataAccessLayer.Models.FiltrationModels
{
   public class OrderFiltrationModelDAL : BaseFiltrationModelDAL
    {
        public string Description { get; set; }//TODO wrong spelling+++ 
        public long? UserId { get; set; }
        public OrderStatusState? Status { get; set; }
    }
}
