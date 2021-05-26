using static Store.DataAccessLayer.Enums.Enums;

namespace Store.DataAccessLayer.Models.FiltrationModels
{
   public class OrderFiltrationModelDAL : BaseFiltrationModelDAL
    {
        public string Discription { get; set; }
        public long? UserId { get; set; }
        public OrderStatusState? Status { get; set; }
        public long? EditionId { get; set; }
    }
}
