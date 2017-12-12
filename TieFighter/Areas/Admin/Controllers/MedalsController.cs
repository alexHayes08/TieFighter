using Google.Cloud.Datastore.V1;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using TieFighter.Areas.Admin.Models.JsViewModels;
using TieFighter.Areas.Admin.Models.MedalsViewModels;
using TieFighter.Models;

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
            //var lastUpdated = DateTime.Now;
            //var vm = new PaginatedMedalsVM()
            //{
            //    LastSynced = lastUpdated,
            //    Medals = Startup.DatastoreDb.GetPaginatedMedals(0, 10, DateTime.Now)
            //};

            var query = new Query(nameof(Medal));
            var entities = Startup.DatastoreDb.Db.RunQuery(query).Entities;
            var medals = new List<Medal>();
            foreach (var entity in entities)
            {
                medals.Add(new Medal().FromEntity(entity) as Medal);
            }

            return View(medals);
        }

        // GET: Medals/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Medals/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([FromBody]Medal medal, IFormCollection collection)
        {
            try
            {
                medal.Id = medal.GenerateNewKey(Startup.DatastoreDb.Db).ToId();
                var entity = DatastoreHelpers.ObjectToEntity(Startup.DatastoreDb, medal, nameof(Medal.Id));

                return RedirectToAction(nameof(Edit), medal.Id);
            }
            catch
            {
                ViewBag.Error = "Failed to create new medal";
                return View();
            }
        }

        // GET: Medals/Edit/5
        public ActionResult Edit(long id)
        {
            if (id == 0)
            {
                Redirect(nameof(Index));
            }

            var key = Startup.DatastoreDb.MedalsKeyFactory.CreateKey(id);
            var response = Startup.DatastoreDb.Db.Lookup(key);

            if (response == null)
            {
                ViewBag.Error = "Failed to find medal";
                return Index();
            }
            else
            {
                var medal = new Medal().FromEntity(response) as Medal;
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
        [ValidateAntiForgeryToken]
        public JsonResult IsIdAvailable(IFormCollection collection)
        {
            var id = collection["Id"];
            var key = Startup.DatastoreDb.MedalsKeyFactory.CreateKey(id);
            var result = Startup.DatastoreDb.Db.Lookup(key);
            //var newAntiForgeToken = Html.AntiForgeryToken();
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
        public JsonResult Delete(IFormCollection collection)
        {
            try
            {
                var ids = collection.Keys;
                var keys = new List<Key>();
                foreach (var id in ids)
                {
                    keys.Add(Startup.DatastoreDb.MedalsKeyFactory.CreateKey(id));
                }

                Startup.DatastoreDb.Db.Delete(keys);

                return Json(new JsDefault()
                {
                    Error = "",
                    Succeeded = true
                });
            }
            catch (Exception e)
            {
                return Json(new JsDefault()
                {
                    Error = e.ToString(),
                    Succeeded = false
                });
            }
        }
    }
}