using Microsoft.AspNetCore.Identity;

namespace Simple_E_commers_App.Models
{
    public class AppUser : IdentityUser<int>
    {
        public string Name { get; set; }
        public string? Address { get; set; }
        // Add CreatedAt and UpdatedAt properties
        public DateTime CreatedAt { get; set; }
        public List<Order> Orders { get; set; }
        public Cart Cart { get; set; }
        public int CartId { get; set; } 
    }

}
