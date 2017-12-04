using Microsoft.AspNetCore.Identity;
using System;

namespace TieFighter.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string DisplayName { get; set; }
        public double DisplayLevel { get; set; }
        public string Thumbnail { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime MostRecentActivity { get; set; }
    }
}
