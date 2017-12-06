using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using TieFighter.Models;

namespace TieFighter.Areas.Admin.Models.UsersViewModels
{
    public class UserWithRolesVM
    {
        public ApplicationUser User { get; set; }
        public IList<UserRole> Roles { get; set; }
    }

    public class UserRole
    {
        public string RoleName { get; set; }
        public bool IsInRole { get; set; }
    }
}
