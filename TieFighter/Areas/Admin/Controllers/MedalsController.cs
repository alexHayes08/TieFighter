using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using TieFighter.Areas.Admin.Models.MedalsViewModels;
using TieFighter.Models;

namespace TieFighter.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class MedalsController : Controller
    {
        // GET: Medals
        public ActionResult Index()
        {
            var lastUpdated = DateTime.Now;
            var vm = new PaginatedMedalsVM()
            {
                LastSynced = lastUpdated,
                Medals = Startup.DatastoreDb.GetPaginatedMedals(0, 10, DateTime.Now)
            };

            return View(vm);
        }

        // GET: Medals/Details/5
        public ActionResult Details(string id)
        {
            return View();
        }

        // GET: Medals/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Medals/Create
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

        // GET: Medals/Edit/5
        public ActionResult Edit(string id)
        {
            var key = Startup.DatastoreDb.MedalsKeyFactory.CreateKey(id);

            var response = Startup.DatastoreDb.Db.Lookup(key);

            if (response == null)
            {
                return Index();
            }
            else
            {
                var medal = DatastoreHelpers.ParseEntityToObject<Medal>(response);
                return View(medal);
            }
        }

        // POST: Medals/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string id, List<IFormFile> files, FormCollection collection)
        {
            try
            {
                var key = Startup.DatastoreDb.MedalsKeyFactory.CreateKey(id);
                var response = Startup.DatastoreDb.Db.Lookup(key);

                if (response == null)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    var medal = DatastoreHelpers.ParseEntityToObject<Medal>(response);

                    //var fileLoc = collection["FileLocation"];

                    return RedirectToAction(nameof(Index));
                }
            }
            catch
            {
                return View();
            }
        }

        // GET: Medals/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Medals/Delete/5
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