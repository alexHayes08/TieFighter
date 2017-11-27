using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Threading.Tasks;
using TieFighter.Models;
using TieFighter.Models.HomeViewModels;

namespace TieFighter.Controllers
{
    public class HomeController : Controller
    {
        //public HomeController(UserManager<ApplicationUser> userManager)
        //{
        //    _userManager = userManager;
        //}

        //private readonly UserManager<ApplicationUser> _userManager;

        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        [AllowAnonymous]
        public IActionResult ArtWork()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult Error()
        {
            return View();
        }
    }
}
