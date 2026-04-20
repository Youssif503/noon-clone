using Microsoft.AspNetCore.Identity;
namespace noon.Domain.Models.Identity
{
    public class User:IdentityUser
    {
        public string First_Name { get; set; }
        public string Last_Name { get; set; } 
        public List<ReView>? ReViews { get; set; } = new();
        public List<Order>? Orders { get; set; } = new();
        public List<Address>? Addresses { get; set; } = new();
        public Cart? Cart { get; set; }
    }
}
