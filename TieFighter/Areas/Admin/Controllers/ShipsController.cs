using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Cloud.Datastore.V1;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using TieFighter.Areas.Admin.Models.JsViewModels;
using TieFighter.Models;

namespace TieFighter.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class ShipsController : Controller
    {
        #region Constructors

        public ShipsController(TieFighterDatastoreContext datastoreContext)
        {
            _datastoreContext = datastoreContext;
        }

        #endregion

        #region Fields

        private TieFighterDatastoreContext _datastoreContext;

        #endregion

        // GET: Ships
        public ActionResult Index()
        {
            var shipsQuery = new Query(nameof(Ship));
            var ships = DatastoreHelpers.ParseEntitiesToObject<Ship>(
                _datastoreContext.Db.RunQuery(shipsQuery).Entities
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
                var shipKey = _datastoreContext.GetKeyFactoryFor(nameof(Ship)).CreateKey(id);
                var ship = new Ship().FromEntity(
                    _datastoreContext.Db.Lookup(shipKey)
                ) as Ship;

                var submeshes = new List<Submesh>();
                var submeshesQuery = new Query(nameof(Submesh))
                {
                    Filter = Filter.Equal(nameof(Submesh.ShipId), ship.Id)
                };
                var submeshEntities = _datastoreContext.Db.RunQuery(submeshesQuery).Entities;
                foreach (var entity in submeshEntities)
                {
                    var submesh = new Submesh().FromEntity(entity) as Submesh;
                    submeshes.Add(submesh);
                }

                ship.Submeshes = submeshes.ToArray();

                return View(ship);
            }
            catch
            {
                ViewBag.Error = "Failed to find ship!";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Ships/Edit/5
        [HttpPost]
        public JsonResult Edit(long id, [FromForm]Ship ship, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
                var key = _datastoreContext.ShipsKeyFactory.CreateKey(id);
                //var ship = DatastoreHelpers.ParseEntityToObject<Ship>(
                //    Startup.DatastoreDb.Db.Lookup(key)
                //);

                // Set display name
                if (!string.IsNullOrEmpty(collection[nameof(Ship.DisplayName)]))
                {
                    ship.DisplayName = collection[nameof(Ship.DisplayName)];
                }

                // Update ship
                var shipEntity = DatastoreHelpers.ObjectToEntity(_datastoreContext, ship);
                _datastoreContext.Db.Update(shipEntity);

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
        public JsonResult UpdateShipModel(long id, IFormCollection collection)
        {
            try
            {
                if (collection.Files.Count == 0)
                    throw new Exception("No file chosen.");
                else if (collection.Keys.Count == 0)
                    throw new Exception("File had no meshes it it.");

                var ship = new Ship().FromEntity(
                    _datastoreContext.Db.Lookup(
                        _datastoreContext.ShipsKeyFactory.CreateKey(id)
                    )
                ) as Ship;

                //ship.SetSubmeshes();

                //if (ship.Submeshes.Length != collection.Count)
                //    ship.DeleteSubmeshes();

                foreach (var key in collection.Keys)
                {
                    var jobj = JObject.Parse(collection[key]);

                    // Create new submeshes
                    var submesh = new Submesh()
                    {
                        MeshName = key,
                        ShipId = ship.Id.Value,
                        RotationOffset = new ThreeDimensionsCoord().FromJObject(jobj[nameof(Submesh.RotationOffset)] as JObject) as ThreeDimensionsCoord,
                        TranslationOffset = new ThreeDimensionsCoord().FromJObject(jobj[nameof(Submesh.TranslationOffset)] as JObject) as ThreeDimensionsCoord,
                        ScaleOffset = new ThreeDimensionsCoord().FromJObject(jobj[nameof(Submesh.ScaleOffset)] as JObject) as ThreeDimensionsCoord
                    };

                    submesh.Save(_datastoreContext.Db);
                }

                ship.UpdateFileAsync(collection.Files);

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
                    keys.Add(_datastoreContext.ShipsKeyFactory.CreateKey(id));
                }

                _datastoreContext.Db.Delete(keys);

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