using noon.Domain.Helpers;
using noon.Domain.Models.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace noon.Domain.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public double TotalPrice { get; set; }
        public OrderStatus Status { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public List<OrderItem> Items { get; set; }
    }
}
