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
using TieFighter.Areas.Admin.Models.JsViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

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
                double pointsWorth = 0;
                double.TryParse(collection[nameof(Medal.PointsWorth)], out pointsWorth);
                var medal = new Medal()
                {
                    Id = collection[nameof(Medal.Id)],
                    MedalName = collection[nameof(Medal.MedalName)],
                    PointsWorth = pointsWorth
                    //Conditions = new string[0]
                };

                Startup.DatastoreDb.MedalsKeyFactory.CreateKey(medal.Id);
                var entity = DatastoreHelpers.ObjectToEntity<Medal>(Startup.DatastoreDb, medal, nameof(Medal.Id));

                return RedirectToAction(nameof(Edit), pointsWorth);
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

        [HttpPost]
        public JsonResult Update(string id, IFormCollection collection)
        { 
            var key = Startup.DatastoreDb.MedalsKeyFactory.CreateKey(id);
            var response = Startup.DatastoreDb.Db.Lookup(key);

            if (response == null)
            {
                return Json(new JsDefault() { Error = $"No such entity with an id {id} was found.", Succeeded = false });
            }
            else
            {
                var medal = DatastoreHelpers.ParseEntityToObject<Medal>(response);

                // Check that there are files and that they're not empty
                if (collection.Files?[0].Length > 0)
                {
                    // Only use the first file
                    var file = collection.Files[0];

                    if (file.ContentType != "image/png")
                    {
                        return Json(new JsDefault() { Error = "Image must be '.png'!", Succeeded = false });
                    }

                    try
                    {
                        var filename = medal.Id + ".png";
                        var filepath = Path.Combine(_hostingEnvironment.WebRootPath, "Medals", $"{medal.Id}.png");
                        using (var stream = new FileStream(filepath, FileMode.Create))
                        {
                            file.CopyTo(stream);
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

                return Json(new JsDefault() { Error = "", Succeeded = true });
            }
        }

        [HttpPost]
        public JsonResult IsIdAvailable(string id)
        {
            var key = Startup.DatastoreDb.MedalsKeyFactory.CreateKey(id);
            var result = Startup.DatastoreDb.Db.Lookup(key);
            if (result != null)
            {
                return Json(new JsExists() { Error = "", Exists = true, Succeeded = false });
            }
            else
            {
                return Json(new JsExists{ Error = "", Exists = false, Succeeded = true });
            }
        }

        // POST: Medals/Delete/5
        [HttpPost]
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