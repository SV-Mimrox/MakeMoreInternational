using MakeMoreInternational.Models;
using MakeMoreInternational.Services;
using Microsoft.AspNetCore.Mvc;

namespace MakeMoreInternational.Areas.ho.Controllers
{
    [Area("ho")]
    [Route("ho/certificate")]
    public class CertificateController : Controller
    {
        private readonly CertificateService _service;
        private readonly IWebHostEnvironment _env;

        public CertificateController(CertificateService service, IWebHostEnvironment env)
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
        public IActionResult Create(CertificateMaster cert, IFormFile crtImageFile, IFormFile crtCertiImageFile)
        {
            try
            {
                if (crtImageFile != null)
                {
                    string path = Path.Combine(_env.WebRootPath, "images/certificate");
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);

                    string fileName = Guid.NewGuid() + Path.GetExtension(crtImageFile.FileName);
                    string fullPath = Path.Combine(path, fileName);
                    using (var fs = new FileStream(fullPath, FileMode.Create))
                    {
                        crtImageFile.CopyTo(fs);
                    }

                    cert.crtImage = fileName;
                }

                if (crtCertiImageFile != null)
                {
                    string path = Path.Combine(_env.WebRootPath, "images/certificate");
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);

                    string fileName = Guid.NewGuid() + Path.GetExtension(crtCertiImageFile.FileName);
                    string fullPath = Path.Combine(path, fileName);
                    using (var fs = new FileStream(fullPath, FileMode.Create))
                    {
                        crtCertiImageFile.CopyTo(fs);
                    }

                    cert.crtCertiImage = fileName;
                }


                _service.Create(cert);
                return RedirectToAction("Index");
            }
            catch (Exception ex) {
                return View(cert);
            }
            
        }

        [HttpGet("edit/{id}")]
        public IActionResult Edit(string id)
        {
            var data = _service.GetById(id);
            return View(data);
        }

        [HttpPost("edit/{id}")]
        public IActionResult Edit(string id, CertificateMaster cert, IFormFile crtImageFile, IFormFile crtCertiImageFile)
        {
            try
            {
                var existing = _service.GetById(id);

                if (crtImageFile != null)
                {
                    string path = Path.Combine(_env.WebRootPath, "images/certificate");
                    string fileName = Guid.NewGuid() + Path.GetExtension(crtImageFile.FileName);
                    using (var fs = new FileStream(Path.Combine(path, fileName), FileMode.Create))
                    {
                        crtImageFile.CopyTo(fs);
                    }
                    cert.crtImage = fileName;
                }
                else
                {
                    cert.crtImage = existing.crtImage;
                }

                if (crtCertiImageFile != null)
                {
                    string path = Path.Combine(_env.WebRootPath, "images/certificate");
                    string fileName = Guid.NewGuid() + Path.GetExtension(crtCertiImageFile.FileName);
                    using (var fs = new FileStream(Path.Combine(path, fileName), FileMode.Create))
                    {
                        crtCertiImageFile.CopyTo(fs);
                    }
                    cert.crtCertiImage = fileName;
                }
                else
                {
                    cert.crtCertiImage = existing.crtCertiImage;
                }


                cert.crtId = id;
                _service.Update(id, cert);
                return RedirectToAction("Index");

            }
            catch(Exception ex)
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
