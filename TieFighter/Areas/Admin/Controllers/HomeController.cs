using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using TieFighter.Data;
using TieFighter.Models;
using static TieFighter.Extensions.ExtensionMethods;

namespace TieFighter.Areas.Admin.Controllers
{
    [Authorize(Roles = "Administrator")]
    [Area("Admin")]
    public class HomeController : Controller
    {
        public HomeController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;

        public IActionResult Index()
        {
            ViewBag.Admin = true;
            return View();
        }

        public IActionResult Users()
        {
            ViewBag.Admin = true;
            Func<ApplicationUser, string> sortByUser = (el => el.Id);
            var users = GetPaginatedItems(0, 10, false, sortByUser, _userManager.Users);
            return View(users);
        }

        public IActionResult Tours()
        {
            ViewBag.Admin = true;
            Func<Tour, int> sortByTour = (el => el.TourID);
            var tours = GetPaginatedItems(0, 10, false, sortByTour, _context.Tours);
            return View(tours);
        }


    }
}