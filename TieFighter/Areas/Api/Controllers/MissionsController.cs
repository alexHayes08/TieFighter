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

namespace TieFighter.Areas.Api.Controllers
{
    [Authorize]
    [Area("Api")]
    public class MissionsController : Controller
    {
        #region Fields

        private readonly TieFighterDatastoreContext _datastoreContext;

        #endregion

        #region Constructor(s)

        public MissionsController(TieFighterDatastoreContext datastoreContext)
        {
            _datastoreContext = datastoreContext;
        }

        #endregion

        // GET: api/Missions
        [HttpGet]
        public JsonResult Get()
        {
            try
            {
                var missionQuery = new Query(nameof(Mission));
                var missions = DatastoreHelpers.ParseEntitiesToObject<Mission>(
                    _datastoreContext.Db.RunQuery(missionQuery).Entities
                );
                var missionObjList = new List<object>(missions);

                return Json(new JsResults()
                {
                    Succeeded = true,
                    Error = "",
                    Results = missionObjList
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

        // GET: api/Missions/5
        [HttpGet("Get/{id}")]
        public JsonResult Get(long id)
        {
            try
            {
                var key = _datastoreContext.MissionsKeyFactory.CreateKey(id);
                var mission = DatastoreHelpers.ParseEntityToObject<Mission>(
                    _datastoreContext.Db.Lookup(key)
                );

                return Json(new JsResult()
                {
                    Succeeded = true,
                    Error = "",
                    Result = mission
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
