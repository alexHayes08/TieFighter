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
        // GET: Game
        public ActionResult Index()
        {
            var gamesQuery = new Query(nameof(Game));
            var entities = Startup.DatastoreDb.Db.RunQuery(gamesQuery).Entities;
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
                reservedKey = game.GenerateNewKey(Startup.DatastoreDb.Db);
                game.Id = reservedKey.ToId();

                var entity = DatastoreHelpers.ObjectToEntity(Startup.DatastoreDb, game);
                Startup.DatastoreDb.Db.Upsert(entity);

                return Redirect($"{nameof(Edit)}/{game.Id.ToString()}");
            }
            catch (Exception e)
            {
                if (reservedKey != null)
                {
                    Startup.DatastoreDb.Db.Delete(reservedKey);
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
                var key = Startup.DatastoreDb.GamesKeyFactory.CreateKey(id);
                var entity = Startup.DatastoreDb.Db.Lookup(key);
                var game = new Game().FromEntity(entity) as Game;

                return View(game);
            }
            catch (Exception e)
            {
                ViewBag.Error = e.ToString();
                return Redirect(nameof(Index));
            }
        }

        // POST: Game/Edit/5
        [HttpPost]
        public JsonResult Edit(long id, IFormCollection collection)
        {
            try
            {
                var game = new Game()
                {
                    Id = id,
                    ImageUrl = collection[nameof(Game.ImageUrl)],
                    IsEnabled = bool.Parse(collection[nameof(Game.IsEnabled)]),
                    Name = collection[nameof(Game.Name)]
                };

                var entity = DatastoreHelpers.ObjectToEntity(Startup.DatastoreDb, game);
                Startup.DatastoreDb.Db.Upsert(entity);

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
                        keys.Add(Startup.DatastoreDb.GamesKeyFactory.CreateKey(key));
                    }
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