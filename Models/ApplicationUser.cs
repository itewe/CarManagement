using Microsoft.AspNetCore.Identity;

namespace CarManagement.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";

    }
}
