using System;
using System.Collections.Generic;
using System.Text;

namespace noon.Domain.Models
{
    public class ProductImages
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public bool isMain { get; set; }
        public int ProductId { get; set; }
        public Product? Product{ get; set; }
    }
}
