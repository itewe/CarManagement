using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace CarManagement.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string FirstName { get; set; } = "";

        [Required]
        public string LastName { get; set; } = "";

        [EmailAddress]
        public override string Email { get; set; }

    }
}
