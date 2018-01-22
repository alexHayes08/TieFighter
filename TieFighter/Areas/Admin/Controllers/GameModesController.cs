using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Cloud.Datastore.V1;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TieFighter.Areas.Admin.Models.JsViewModels;
using TieFighter.Models;

namespace TieFighter.Areas.Admin.Controllers
{
    public class GameModesController : Controller
    {
        private TieFighterDatastoreContext _datastoreContext;

        public GameModesController(TieFighterDatastoreContext datastoreContext)
        {
            _datastoreContext = datastoreContext;
        }

        // GET: GameModes
        public ActionResult Index()
        {
            var gamesQuery = new Query(nameof(GameMode));
            var entities = _datastoreContext.Db.RunQuery(gamesQuery).Entities;
            var gameModes = DatastoreHelpers.ParseEntitiesToObject<GameMode>(entities);

            return View(gameModes);
        }

        // GET: GameModes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: GameModes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([FromBody]GameMode gameMode, IFormCollection collection)
        {
            try
            {
                gameMode.Id = gameMode
                    .GenerateNewKey(_datastoreContext.Db)
                    .ToId();
                _datastoreContext.Db.Upsert(gameMode.ToEntity());

                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                ViewBag.Error = e.ToString();
                return View();
            }
        }

        // GET: GameModes/Edit/5
        public ActionResult Edit(long id)
        {
            return View();
        }

        // POST: GameModes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Edit(long id, [FromBody]GameMode gameMode, IFormCollection collection)
        {
            try
            {
                var entity = gameMode.ToEntity();
                _datastoreContext.Db.Upsert(entity);

                return Json(new JsDefault()
                {
                    Succeeded = true,
                    Error = ""
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

        // POST: GameModes/Delete/5
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
                    {
                        keys.Add(_datastoreContext.GameModesFactory.CreateKey(key));
                    }
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