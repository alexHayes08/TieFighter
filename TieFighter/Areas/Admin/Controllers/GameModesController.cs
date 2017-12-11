using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Cloud.Datastore.V1;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TieFighter.Models;

namespace TieFighter.Areas.Admin.Controllers
{
    public class GameModesController : Controller
    {
        // GET: GameModes
        public ActionResult Index()
        {
            var gamesQuery = new Query(nameof(GameMode));
            var entities = Startup.DatastoreDb.Db.RunQuery(gamesQuery).Entities;
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
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                var gameMode = new GameMode()
                {
                    Id = new Guid().ToString(),
                    Name = collection[nameof(GameMode.Name)]
                };

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: GameModes/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: GameModes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // POST: GameModes/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
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