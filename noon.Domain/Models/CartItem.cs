using System;
using System.Collections.Generic;
using System.Text;

namespace noon.Domain.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; } 
        public int CartId { get; set; }
        public Cart Cart { get; set; }
    }
}
