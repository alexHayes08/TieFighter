using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using TieFighter.Models;

namespace TieFighter.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class Users2Controller : Controller
    {
        public Users2Controller(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        // GET: api/Users2
        [Route("/api/Users")]
        [HttpGet]
        public JsonResult Get()
        {
            return Json(_userManager.Users.ToList());
        }

        // GET: api/Users2/5
        [Route("/api/Users")]
        [HttpGet("{id}", Name = "Get")]
        public JsonResult Get(int id)
        {
            return Json(_userManager.Users.Where(u => u.Id == id.ToString()).FirstOrDefault());
        }

        // POST: api/Users2
        [Route("/api/Users")]
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Users2/5
        [Route("/api/Users")]
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [Route("/api/Users")]
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            try
            {
                _userManager.DeleteAsync(_userManager.Users.Where(u => u.Id == id.ToString()).FirstOrDefault());
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
