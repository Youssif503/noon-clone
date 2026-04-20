using noon.Domain.Models.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace noon.Domain.Models
{
    public class ReView
    {
        public int Id { get; set; }
        public int ReviewRate {  get; set; }
        public string ReviewText { get; set; } = string.Empty;
        public int ProductId { get; set; }
        public Product? Product { get; set; }
        public string? UserId { get; set; }
        public User? User { get; set; }

    }
}
