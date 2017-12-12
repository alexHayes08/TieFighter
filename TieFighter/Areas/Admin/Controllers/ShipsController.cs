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
        public ActionResult Edit(long id)
        {
            try
            {
                var shipKey = Startup.DatastoreDb.ShipsKeyFactory.CreateKey(id);
                var ship = DatastoreHelpers.ParseEntityToObject<Ship>(
                    Startup.DatastoreDb.Db.Lookup(shipKey)
                );

                var submeshes = new List<Submesh>();
                var submeshesQuery = new Query(nameof(Submesh))
                {
                    Filter = Filter.Equal(nameof(Submesh.ShipId), ship.Id)
                };
                var submeshEntities = Startup.DatastoreDb.Db.RunQuery(submeshesQuery).Entities;
                foreach (var entity in submeshEntities)
                {
                    var submesh = new Submesh().FromEntity(entity) as Submesh;
                    submeshes.Add(submesh);
                }

                return View(ship);
            }
            catch
            {
                ViewBag.Error = "Failed to find ship!";
                return Index();
            }
        }

        // POST: Ships/Edit/5
        [HttpPost]
        public JsonResult Edit(long id, IFormCollection collection)
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

        // Submitting an array of submeshes
        [HttpPost]
        public JsonResult UpdateShipInfo([FromBody]Submesh[] submeshes)
        {
            try
            {
                

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
                    Error = "",
                    Succeeded = true
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