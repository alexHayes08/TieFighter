using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TieFighter.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class ResourceManagerController : Controller
    {
        #region Constructors

        #endregion

        #region Fields

        #endregion

        // GET: ResourceManager
        public ActionResult Index()
        {
            return View();
        }

        // GET: ResourceManager/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ResourceManager/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ResourceManager/Create
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

        // GET: ResourceManager/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ResourceManager/Edit/5
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

        // GET: ResourceManager/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ResourceManager/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}