using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using TieFighterV2.Models;

namespace TieFighter.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string DisplayName { get; set; }
        public double DisplayLevel { get; set; }
        public string Thumbnail { get; set; }

        public ICollection<Medals> Medals { get; set; }
    }
}
