using Microsoft.AspNetCore.Identity;
namespace noon.Domain.Models.Identity
{
    public class User:IdentityUser<int>
    {
        public string First_Name { get; set; }
        public string Last_Name { get; set; } 
        public string phone { get; set; }
        public List<ReView>? ReViews { get; set; } = new();
        public List<Order>? Orders { get; set; } = new();
        public List<Address>? Addresses { get; set; } = new();
        public int? CartId { get; set; }
        public Cart Cart { get; set; }
    }
}
