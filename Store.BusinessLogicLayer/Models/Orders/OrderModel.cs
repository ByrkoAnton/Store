﻿using Store.BusinessLogicLayer.Models.OrderItems;
using System.Collections.Generic;
using static Store.DataAccessLayer.Enums.Enums;

namespace Store.BusinessLogicLayer.Models.Orders
{
    public class OrderModel
    {
        public long Id { get; set; }
        public string Discription { get; set; }
        public long UserId { get; set; }
        public long PaymentId { get; set; }
        public OrderStatus Status { get; set; }
        public List<OrderItemModel> OrderItems { get; set; }
    }
}
