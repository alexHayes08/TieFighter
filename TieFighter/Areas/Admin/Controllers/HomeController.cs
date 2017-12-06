using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TieFighter.Models;
using TieFighter.Areas.Admin.Models;
using System.Linq;
using TieFighter.Models.HomeViewModels;
using System.Collections.Generic;
using Google.Cloud.Datastore.V1;
using static Google.Cloud.Datastore.V1.PropertyOrder.Types;

namespace TieFighter.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class HomeController : Controller
    {
        public HomeController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, TieFighterContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly TieFighterContext _context;

        public IActionResult Index()
        {
            var dashboard = new DashboardVM()
            {
                LatestUsers = new List<UserGameViewModel>(),
                MostPopularMissions = new List<Mission>(),
                MostPopularShips = new List<Ship>()
            };

            // Get 5 most recent users
            var mostRecentUsers = _userManager.Users.OrderBy(u => u.CreatedOn).ToList();
            foreach (var usr in mostRecentUsers)
            {
                dashboard.LatestUsers.Add(new UserGameViewModel()
                {
                    CreatedOn = usr.CreatedOn,
                    DisplayLevel = usr.DisplayLevel,
                    DisplayName = usr.DisplayName,
                    Email = usr.Email,
                    Thumbnail = usr.Thumbnail,
                    Uid = usr.Id
                });
            }

            // Get most popular maps
            var popularMapsQuery = new Query(nameof(Mission))
            {
                Limit = 5,
                Order = { { nameof(Mission.LastPlayedOn), Direction.Descending } }
            };
            var entities = Startup.DatastoreDb.Db.RunQuery(popularMapsQuery).Entities;
            dashboard.MostPopularMissions = DatastoreHelpers
                .ParseEntitiesToObject<Mission>(entities)
                .OrderBy(m => m.LastPlayedOn)
                .ToList();
            
            // TODO: Get most popular ships

            return View(dashboard);
        }
    }
}