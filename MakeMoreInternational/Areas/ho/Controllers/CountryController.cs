using MakeMoreInternational.Models;
using MakeMoreInternational.Services;
using Microsoft.AspNetCore.Mvc;

namespace MakeMoreInternational.Areas.ho.Controllers
{
    [Area("ho")]
    [Route("ho/country")]
    public class CountryController : Controller
    {
        private readonly CountryMasterService _service;
        private readonly IWebHostEnvironment _env;

        public CountryController(CountryMasterService service, IWebHostEnvironment env)
        {
            _service = service;
            _env = env;
        }
        [HttpGet("")]
        public IActionResult Index()
        {
            var data = _service.GetAll();
            return View(data);
        }

        [HttpGet("create")]
        public IActionResult Create() => View();

        [HttpPost("create")]
        public IActionResult Create(CountryMaster cert, IFormFile cntImageFile)
        {
            try
            {
                if (cntImageFile != null)
                {
                    string path = Path.Combine(_env.WebRootPath, "images/countries");
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);

                    string fileName = Guid.NewGuid() + Path.GetExtension(cntImageFile.FileName);
                    string fullPath = Path.Combine(path, fileName);
                    using (var fs = new FileStream(fullPath, FileMode.Create))
                    {
                        cntImageFile.CopyTo(fs);
                    }

                    cert.cntImage = fileName;
                }

                _service.Create(cert);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View(cert);
            }

        }

       

        [HttpGet("delete/{id}")]
        public IActionResult Delete(string id)
        {
            _service.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
