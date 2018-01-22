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
        #region Fields

        private readonly TieFighterDatastoreContext _datastoreContext;

        #endregion

        #region Constructor(s)

        public ShipComponentsController(TieFighterDatastoreContext datastoreContext)
        {
            _datastoreContext = datastoreContext;
        }

        #endregion

        // GET: ShipComponents
        public ActionResult Index()
        {
            var query = new Query(nameof(ShipComponent));
            var entities = _datastoreContext.Db.RunQuery(query).Entities;
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
                shipComponent.Id = shipComponent.GenerateNewKey(_datastoreContext.Db).ToId();
                _datastoreContext.Db.Upsert(shipComponent.ToEntity());

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
            var shipComponent = new ShipComponent().FromEntity(
                _datastoreContext.Db.Lookup(
                    _datastoreContext.ShipComponentsKeyFactory.CreateKey(id)
                )
            );

            return View(shipComponent);
        }

        // POST: ShipComponents/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Edit(long id, [FromForm]ShipComponent shipComponent, IFormCollection collection)
        {
            try
            {
                _datastoreContext.Db.Upsert(shipComponent.ToEntity());

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
                        keys.Add(_datastoreContext.ShipComponentsKeyFactory.CreateKey(id));
                }

                _datastoreContext.Db.Delete(keys);

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