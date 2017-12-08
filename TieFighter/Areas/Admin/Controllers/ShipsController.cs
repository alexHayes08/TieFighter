using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Cloud.Datastore.V1;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TieFighter.Areas.Admin.Models.JsViewModels;
using TieFighter.Models;

namespace TieFighter.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class ShipsController : Controller
    {
        // GET: Ships
        public ActionResult Index()
        {
            var shipsQuery = new Query(nameof(Ship));
            var ships = DatastoreHelpers.ParseEntitiesToObject<Ship>(
                Startup.DatastoreDb.Db.RunQuery(shipsQuery).Entities
            );
            return View(ships);
        }

        // GET: Ships/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Ships/Create
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

        // GET: Ships/Edit/5
        public ActionResult Edit(string id)
        {
            try
            {
                var shipKey = Startup.DatastoreDb.ShipsKeyFactory.CreateKey(id);
                var ship = DatastoreHelpers.ParseEntityToObject<Ship>(
                    Startup.DatastoreDb.Db.Lookup(shipKey)
                );
                return View(ship);
            }
            catch
            {
                return Index();
            }
        }

        // POST: Ships/Edit/5
        [HttpPost]
        public JsonResult Edit(string id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
                var key = Startup.DatastoreDb.ShipsKeyFactory.CreateKey(id);
                var ship = DatastoreHelpers.ParseEntityToObject<Ship>(
                    Startup.DatastoreDb.Db.Lookup(key)
                );

                // Set display name
                if (!string.IsNullOrEmpty(collection[nameof(Ship.DisplayName)]))
                {
                    ship.DisplayName = collection[nameof(Ship.DisplayName)];
                }

                // Update ship
                var shipEntity = DatastoreHelpers.ObjectToEntity(Startup.DatastoreDb, ship, nameof(Ship.Id));
                Startup.DatastoreDb.Db.Update(shipEntity);

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

        // POST: Ships/Delete/5
        [HttpPost]
        public JsonResult Delete(IFormCollection collection)
        {
            try
            {
                var keys = new List<Key>();
                foreach (var id in collection.Keys)
                {
                    keys.Add(Startup.DatastoreDb.ShipsKeyFactory.CreateKey(id));
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