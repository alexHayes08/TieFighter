using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TieFighter.Areas.Admin.Models.JsViewModels;

namespace TieFighter.Areas.Admin
{
    public class SystemController : Controller
    {
        #region Constructors

        public SystemController(ILogger<SystemController> logger)
        {
            _logger = logger;
        }

        #endregion

        #region Fields

        private readonly ILogger _logger;

        #endregion

        // GET: System
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Log()
        {
            // TODO
            return View();
        }

        [HttpGet]
        public ActionResult Configuration()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Configuration(IFormCollection collection)
        {
            return Json(new JsDefault()
            {
                Error = "",
                Succeeded = true
            });
        }

        public ActionResult Plugins()
        {
            return View();
        }
    }
}