using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using TieFighter.Areas.Admin.Models.JsViewModels;

namespace TieFighter.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class SystemController : Controller
    {
        private IHostingEnvironment _hostingEnvironment;
        private const string resourcesSubPath = "";

        private IDirectoryContents ResourceDirectoryContents
        {
            get
            {
                return _hostingEnvironment.ContentRootFileProvider.GetDirectoryContents(resourcesSubPath);
            }
        }

        public SystemController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index()
        {
            var files = ResourceDirectoryContents;

            return View(files);
        }

        public async Task<JsonResult> AddResources(IList<IFormFile> files)
        {
            var json = new JsMultipleResults();

            foreach (var file in files)
            {
                try
                {
                    var path = Path.Combine(_hostingEnvironment.ContentRootPath, resourcesSubPath, file.FileName);
                    if (file.Length > 0)
                    {
                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }
                    }

                    json.Results.Add(new JsDefault { Succeeded = true, Message = file.Name });
                }
                catch (Exception e)
                {
                    json.Results.Add(new JsDefault
                    {
                        Succeeded = false,
                        Error = e.ToString(),
                        Message = file.Name
                    });
                }
            }

            return Json(json);
        }

        public async Task<JsonResult> RemoveResource(string resourceName)
        {
            throw new NotImplementedException();
        }

        public async Task<IFormFile> GetResource(string resourceName)
        {
            throw new NotImplementedException();
        }
    }
}