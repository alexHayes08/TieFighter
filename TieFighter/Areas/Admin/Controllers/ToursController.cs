using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Google.Cloud.Datastore.V1;
using TieFighter.Models;
using Microsoft.AspNetCore.Authorization;
using TieFighter.Areas.Admin.Models.ToursViewModels;

namespace TieFighter.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class ToursController : Controller
    {
        // GET: Tours
        public ActionResult Index()
        {
            var query = new Query(nameof(Tour));
            var entities = Startup.DatastoreDb.Db.RunQuery(query).Entities;
            var tours = DatastoreHelpers.ParseEntitiesToObject<Tour>(entities);
            return View(tours);
        }

        // GET: Tours/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Tours/Create
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

        // GET: Tours/Edit/5
        public ActionResult Edit(string id)
        {
            var tourKey = Startup.DatastoreDb.ToursKeyFactory.CreateKey(id);
            try
            {
                var tour = DatastoreHelpers.ParseEntityToObject<Tour>(Startup.DatastoreDb.Db.Lookup(tourKey));
                var missionsQuery = new Query(nameof(Mission))
                {
                    Filter = Filter.Equal(nameof(Mission.TourId), tour.TourId)
                };
                var missions = DatastoreHelpers.ParseEntitiesToObject<Mission>(
                    Startup.DatastoreDb.Db.RunQuery(missionsQuery).Entities
                );
                tour.Missions = missions;
                var conflictingToursQuery = new Query(nameof(Tour))
                {
                    Filter = Filter.Equal(nameof(Tour.Position), tour.Position)
                };
                var conflictingTours = DatastoreHelpers.ParseEntitiesToObject<Tour>(
                    Startup.DatastoreDb.Db.RunQuery(conflictingToursQuery).Entities
                );
                var sameTour = conflictingTours
                    .Where(t => t.TourId == tour.TourId)
                    .FirstOrDefault();
                conflictingTours.Remove(sameTour);

                var tourWithConflicts = new TourWithMissionVM()
                {
                    Tour = tour,
                    ToursWithConflictingPositions = conflictingTours
                };

                return View(tourWithConflicts);
            }
            catch
            {
                return Redirect(nameof(Index));
            }
        }

        // POST: Tours/Edit/5
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

        // GET: Tours/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Tours/Delete/5
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