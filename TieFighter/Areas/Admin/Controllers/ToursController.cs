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
using TieFighter.Areas.Admin.Models.JsViewModels;

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
                var mission = new Mission()
                {
                    DisplayName = collection[nameof(Mission.DisplayName)],
                    Id = collection[nameof(Mission.Id)],
                    LastPlayedOn = default(DateTime),
                    MissionBriefing = collection[nameof(Mission.MissionBriefing)],
                    PositionInTour = int.Parse(collection[nameof(Mission.PositionInTour)]),
                    TourId = collection[nameof(Mission.TourId)]
                };

                var entity = DatastoreHelpers.ObjectToEntity(Startup.DatastoreDb, mission, nameof(Mission.Id));
                Startup.DatastoreDb.Db.Upsert(entity);

                return RedirectToAction(nameof(EditMission));
            }
            catch (Exception e)
            {
                ViewBag.Error = e.ToString();
                return View();
            }
        }

        [HttpGet]
        public ActionResult CreateMission()
        {
            return View();
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
                var conflictingTours = GetToursWithSamePosition(tour.Position);
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
        public ActionResult Edit(string id, IFormCollection collection)
        {
            try
            {
                var tour = new Tour()
                {
                    Position = int.Parse(collection[nameof(Tour.Position)]),
                    TourId = id,
                    TourName = collection[nameof(Tour.TourName)]
                };

                var entity = DatastoreHelpers.ObjectToEntity(Startup.DatastoreDb, tour, nameof(Tour.TourId));
                Startup.DatastoreDb.Db.Upsert(entity);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [HttpGet]
        public ActionResult EditMission(string id)
        {
            var key = Startup.DatastoreDb.MissionsKeyFactory.CreateKey(id);
            var mission = DatastoreHelpers
                .ParseEntityToObject<Mission>(Startup.DatastoreDb.Db.Lookup(key));

            return View(mission);
        }

        [HttpPost]
        public JsonResult EditMission(string id, IFormCollection collection)
        {
            try
            {
                var mission = new Mission()
                {
                    Id = id,
                    DisplayName = collection["DisplayName"],
                    MissionBriefing = collection["MissionBriefing"],
                    PositionInTour = int.Parse(collection["PositionInTour"])
                };

                var entity = DatastoreHelpers.ObjectToEntity(Startup.DatastoreDb, mission, "Id");
                Startup.DatastoreDb.Db.Update(entity);

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

        [HttpPost]
        public JsonResult ToursWithSamePosition (string id)
        {
            if (int.TryParse(id, out int pos))
            {
                var tours = new List<object>(GetToursWithSamePosition(pos));
                return Json(new JsResults()
                {
                    Succeeded = true,
                    Error = "",
                    Message = "",
                    Results = tours
                });
            }
            else
            {
                return Json(new JsDefault()
                {
                    Error = "Position wasn't a number",
                    Succeeded = false
                });
            }
        }

        private IList<Tour> GetToursWithSamePosition (int position)
        {
            var query = new Query(nameof(Tour))
            {
                Filter = Filter.Equal(nameof(Tour.Position), position)
            };
            var entities = Startup.DatastoreDb.Db.RunQuery(query).Entities;
            var tours = DatastoreHelpers.ParseEntitiesToObject<Tour>(entities);

            return tours;
        }
    }
}