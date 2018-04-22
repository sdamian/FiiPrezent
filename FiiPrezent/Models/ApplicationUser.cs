using Microsoft.AspNetCore.Identity;

namespace FiiPrezent.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
        public string PhotoUrl { get; set; }
    }
}