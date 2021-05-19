using Store.DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Store.DataAccessLayer.Enums.Enums;

namespace Store.DataAccessLayer.Models.FiltrationModels
{
   public class OrderFiltrPagingSortModelDAL : BaseFiltrPagingSortModelDAL
    {
        public string Discription { get; set; }
        public long? UserId { get; set; }
        public OrderStatus? Status { get; set; }
        public long? EditionId { get; set; }
    }
}
