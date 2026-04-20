using noon.Domain.Models.Identity;

namespace noon.Domain.Models
{
    public class Address
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string? Governorate { get; set; }  
        public string? City { get; set; }       
        public string? Area { get; set; }         
        public string? Street { get; set; }   
        public string? Building { get; set; } 
        public string? Floor { get; set; }   
        public string? Apartment { get; set; }   
        public bool? isMain { get; set; }
        public string UserId { get; set; }
        public User? User { get; set; }
    }
}
