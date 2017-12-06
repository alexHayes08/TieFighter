using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TieFighter.Areas.Admin.Models.JsViewModels;
using TieFighter.Areas.Admin.Models.UsersViewModels;
using TieFighter.Models;
using TieFighter.Models.HomeViewModels;

namespace TieFighter.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        #region Contstructor

        public UsersController(TieFighterContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        #endregion

        #region Fields

        private readonly TieFighterContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        #endregion

        // GET: Users
        public ActionResult Index()
        {
            var users = _context.Users.ToList();
            var userVMs = new List<UserGameViewModel>();
            foreach (var user in users)
            {
                userVMs.Add(new UserGameViewModel()
                {
                    DisplayLevel = user.DisplayLevel,
                    DisplayName = user.DisplayName,
                    Email = user.Email,
                    Thumbnail = user.Thumbnail,
                    Uid = user.Id
                });
            }

            return View(userVMs);
        }

        // GET: Users/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Users/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            var user = await _userManager.GetUserAsync(User);
            var allRoles = _roleManager.Roles.ToList();
            var usrRoles = await _userManager.GetRolesAsync(user);
            var roles = new List<UserRole>();
            foreach (var role in allRoles)
            {

                roles.Add(new UserRole()
                {
                    IsInRole = usrRoles.Contains(role.Name),
                    RoleName = role.Name
                });
            }
            var userWithRolesVM = new UserWithRolesVM()
            {
                User = user,
                Roles = roles 
            };
            return View(userWithRolesVM);
        }

        // POST: Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // Post: Users/Delete
        [HttpPost]
        public async Task<JsonResult> Delete(IFormCollection collection)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var userIds = collection.Keys;

            // Users shouldn't be able to delete themselves
            var users = _userManager
                .Users
                .Where(u => userIds.Contains(u.Id) && u.Id != currentUser.Id)
                .ToList();
            try
            {
                foreach (var user in users)
                {
                    _userManager.DeleteAsync(user);
                }

                return Json(new JsDefault() { Error = "", Succeeded = true });
            }
            catch (Exception e)
            {
                return Json(new JsDefault() { Error = e.ToString(), Succeeded = false });
            }
        }
    }
}