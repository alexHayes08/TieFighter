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
    public class ShipComponentsController : Controller
    {
        // GET: ShipComponents
        public ActionResult Index()
        {
            var query = new Query(nameof(ShipComponent));
            var entities = Startup.DatastoreDb.Db.RunQuery(query).Entities;
            var shipComponents = new List<ShipComponent>();
            foreach (var entity in entities)
            {
                shipComponents.Add(new ShipComponent().FromEntity(entity) as ShipComponent);
            }

            return View(shipComponents);
        }

        // GET: ShipComponents/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ShipComponents/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([FromForm]ShipComponent shipComponent, IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here
                shipComponent.Id = shipComponent.GenerateNewKey(Startup.DatastoreDb.Db).ToId();
                Startup.DatastoreDb.Db.Upsert(shipComponent.ToEntity());

                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                ViewBag.Error = e.ToString();
                return View();
            }
        }

        // GET: ShipComponents/Edit/5
        public ActionResult Edit(long id)
        {
            return View();
        }

        // POST: ShipComponents/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Edit(long id, [FromForm]ShipComponent shipComponent, IFormCollection collection)
        {
            try
            {
                Startup.DatastoreDb.Db.Upsert(shipComponent.ToEntity());

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

        // POST: ShipComponents/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Delete(IFormCollection collection)
        {
            try
            {
                var keys = new List<Key>();
                foreach (var key in collection.Keys)
                {
                    if (long.TryParse(key, out long id))
                        keys.Add(Startup.DatastoreDb.ShipComponentsKeyFactory.CreateKey(id));
                }

                Startup.DatastoreDb.Db.Delete(keys);

                return Json(new JsDefault()
                {
                    Error = "",
                    Succeeded = true
                });
            }
            catch(Exception e)
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