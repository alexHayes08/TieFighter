﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Cloud.Datastore.V1;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TieFighter.Areas.Admin.Models.JsViewModels;
using TieFighter.Models;

namespace TieFighter.Areas.Api.Controllers
{
    [Authorize]
    [Area("Api")]
    public class ToursController : Controller
    {
        // GET: api/Tours
        [HttpGet]
        public JsonResult Get()
        {
            try
            {
                var toursQuery = new Query(nameof(Tour));
                var tours = DatastoreHelpers.ParseEntitiesToObject<Tour>(
                    Startup.DatastoreDb.Db.RunQuery(toursQuery).Entities
                );

                var toursObjList = new List<object>(tours);

                return Json(new JsResults()
                {
                    Error = "",
                    Succeeded = true,
                    Results = toursObjList
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

        // GET: api/Tours/5
        [HttpGet("Get/{id}")]
        public JsonResult Get(string id)
        {
            try
            {
                var key = Startup.DatastoreDb.ToursKeyFactory.CreateKey(id);
                var tour = DatastoreHelpers.ParseEntityToObject<Tour>(
                    Startup.DatastoreDb.Db.Lookup(key)
                );

                return Json(new JsResult()
                {
                    Error = "",
                    Succeeded = true,
                    Result = tour
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