using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using TieFighter.Areas.Admin.Models.MedalsViewModels;
using TieFighter.Models;
using System.IO;
using Google.Cloud.Datastore.V1;
using Microsoft.AspNetCore.Hosting;

namespace TieFighter.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class MedalsController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public MedalsController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

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

        // TODO: Make 
        // POST: Medals/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(string id, IFormCollection collection)
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

                    if (collection.Files.Count > 0)
                    {
                        // Only use the first file
                        var file = collection.Files[0];

                        if (file.ContentType != "image/png")
                        {
                            ViewBag.ErrorMessage = "Image must be '.png'!";
                            return View(medal);
                        }

                        try
                        {
                            var filename = medal.Id + ".png";
                            var filepath = Path.Combine(_hostingEnvironment.WebRootPath, "Medals", $"{medal.Id}.png");
                            using (var stream = new FileStream(filepath, FileMode.Create))
                            {
                                await file.CopyToAsync(stream);
                            }

                            medal.FileLocation = filename;
                        }
                        catch (Exception e)
                        { }

                    }

                    // Update the Description
                    if (!string.IsNullOrEmpty(collection[nameof(Medal.Description)]))
                    {
                        medal.Description = collection[nameof(Medal.Description)];
                    }

                    // Update the MedalName
                    if (!string.IsNullOrEmpty(collection[nameof(Medal.MedalName)]))
                    {
                        medal.MedalName = collection[nameof(Medal.MedalName)];
                    }

                    // Update the PointsWorth
                    if (!string.IsNullOrEmpty(collection[nameof(Medal.PointsWorth)]))
                    {
                        if (double.TryParse(collection[nameof(Medal.PointsWorth)], out double newValue))
                        {
                            medal.PointsWorth = newValue;
                        }
                    }

                    // Upsert the medal to Datastore
                    var entity = DatastoreHelpers.ObjectToEntity(Startup.DatastoreDb, medal, nameof(Medal.Id));
                    Startup.DatastoreDb.Db.Upsert(entity);

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