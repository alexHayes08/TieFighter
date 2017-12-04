using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TieFighter.Models;
using TieFighter.Models.GameViewModals;
using TieFighter.Models.HomeViewModels;

namespace TieFighter.Controllers
{
    [Authorize(Policy = Startup.SignedInPolicyName)]
    public class GameController : Controller
    {
        public GameController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        //private TieFighterDatastoreContext db;

        [HttpGet]
        public IActionResult Index()
        {
            var games = new GameTitleViewModal[2]
            {
                new GameTitleViewModal()
                {
                    Title = "TIE-Fighter",
                    Description = "Something something darkside...",
                    IsReleased = true,
                    GameUrl = "/Game/TieFighter",
                    CoverPicture = "/images/SWTieFighter.jpg"
                },
                new GameTitleViewModal()
                {
                    Title = "TIE-Predator",
                    Description = "Something something darkside...",
                    IsReleased = false,
                    GameUrl = "/Game/TiePredator",
                    CoverPicture = "/images/SWTiePredator.jpg"
                }
            };

            return View(games);
        }

        [HttpGet]
        public async Task<IActionResult> TieFighter()
        {
            var user = await _userManager.GetUserAsync(User);
            var userVM = new UserGameViewModel()
            {
                DisplayLevel = user.DisplayLevel,
                DisplayName = user.DisplayName,
                Email = user.Email,
                Thumbnail = user.Thumbnail,
                Uid = user.Id
            };

            return View(userVM);
        }

        [HttpGet]
        public async Task<IActionResult> TiePredator()
        {
            var user = await _userManager.GetUserAsync(User);
            var userVM = new UserGameViewModel()
            {
                DisplayLevel = user.DisplayLevel,
                DisplayName = user.DisplayName,
                Email = user.Email,
                Thumbnail = user.Thumbnail,
                Uid = user.Id
            };

            return View(userVM);
        }
    }
}