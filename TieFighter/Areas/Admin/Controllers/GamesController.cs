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
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class GamesController : Controller
    {
        #region Fields

        private TieFighterDatastoreContext _datastoreContext;

        #endregion

        #region Constructor(s)

        public GamesController(TieFighterDatastoreContext datastoreContext)
        {
            _datastoreContext = datastoreContext;
        }

        #endregion

        // GET: Game
        public ActionResult Index()
        {
            var gamesQuery = new Query(nameof(Game));
            var entities = _datastoreContext.Db.RunQuery(gamesQuery).Entities;
            var games = new List<Game>();
            foreach (var entity in entities)
            {
                games.Add(new Game().FromEntity(entity) as Game);
            }

            return View(games);
        }

        // GET: Game/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Game/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([FromForm]Game game, IFormCollection collection)
        {
            Key reservedKey = null;

            try
            {
                reservedKey = game.GenerateNewKey(_datastoreContext.Db);
                game.Id = reservedKey.ToId();

                //var entity = DatastoreHelpers.ObjectToEntity(Startup.DatastoreDb, game);
                var entity = game.ToEntity();
                _datastoreContext.Db.Upsert(entity);

                return Redirect($"{nameof(Edit)}/{game.Id.ToString()}");
            }
            catch (Exception e)
            {
                if (reservedKey != null)
                {
                    _datastoreContext.Db.Delete(reservedKey);
                }
                ViewBag.Error = "Failed to create game. " + e.ToString();
                return View();
            }
        }

        // GET: Game/Edit/5
        public ActionResult Edit(long id)
        {
            try
            {
                //var key = Startup.DatastoreDb.GamesKeyFactory.CreateKey(id.ToString());
                //var entity = Startup.DatastoreDb.Db.Lookup(key);
                //if (entity == null)
                //{
                //    key = Startup.DatastoreDb.GamesKeyFactory.CreateKey(id);
                //    entity = Startup.DatastoreDb.Db.Lookup(key);
                //}
                var key = _datastoreContext.GamesKeyFactory.CreateKey(id);
                var entity = _datastoreContext.Db.Lookup(key);
                var game = new Game().FromEntity(entity) as Game;

                return View(game);
            }
            catch (Exception e)
            {
                ViewBag.Error = e.ToString();
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Game/Edit/5
        [HttpPost]
        public JsonResult Edit(long id, [FromBody]Game game, IFormCollection collection)
        {
            try
            {
                var entity = DatastoreHelpers.ObjectToEntity(_datastoreContext, game);
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

        // POST: Game/Delete/5
        [HttpPost]
        public JsonResult Delete(IFormCollection collection)
        {
            try
            {
                var keys = new List<Key>();
                foreach (var key in collection.Keys)
                {
                    if (long.TryParse(key, out long id))
                    {
                        keys.Add(_datastoreContext.GamesKeyFactory.CreateKey(key));
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