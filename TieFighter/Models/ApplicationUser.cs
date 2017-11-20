using Microsoft.AspNetCore.Identity;

namespace TieFighter.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string DisplayName { get; set; }
        public double DisplayLevel { get; set; }
        public string Thumbnail { get; set; }
    }
}
