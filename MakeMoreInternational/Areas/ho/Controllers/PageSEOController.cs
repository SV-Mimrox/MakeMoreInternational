using MakeMoreInternational.Models;
using MakeMoreInternational.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MongoDB.Bson;
using System.Runtime.ConstrainedExecution;

namespace MakeMoreInternational.Areas.ho.Controllers
{
    [Area("ho")]
    [Route("ho/page-seo")]
	[AuthorizeByCookie]
	public class PageSEOController : Controller
    {
        private readonly PageSEOService _seoService;
        private readonly IWebHostEnvironment _env;
        public PageSEOController(PageSEOService service, IWebHostEnvironment env)
        {
            _seoService = service;
            _env = env;
        }
        private List<SelectListItem> GetPageOptions()
        {
            return new List<SelectListItem>
        {
            new("Home", "Home"),
            new("About", "About"),
            new("Team", "Teams"),
            new("Infrastructure", "Infrastructure"),
            new("Harvest", "HarvestChart"),
            new("Contact", "Contact"),
            new("Product", "Info"),
            new("Blog", "Blogs"),
            new("Terms", "Terms"),
            new("Privacy", "Privacy"),

        };
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            return View(_seoService.GetAll());
        }

        [HttpGet("create")]
        public IActionResult Create()
        {
            ViewBag.PageList = GetPageOptions();
            return View(new PageSeo());
        }

        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PageSeo model, IFormFile bannerImage)
        {
            if (ModelState.IsValid)
            {
                if (bannerImage != null)
                {
                    string path = Path.Combine(_env.WebRootPath, "images/pages");
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);

                    string fileName = Guid.NewGuid() + Path.GetExtension(bannerImage.FileName);
                    string fullPath = Path.Combine(path, fileName);
                    using (var fs = new FileStream(fullPath, FileMode.Create))
                    {
                        bannerImage.CopyTo(fs);
                    }

                    model.psBanner = fileName;
                }
                _seoService.Create(model);
                return RedirectToAction("Index");
            }
            ViewBag.PageList = GetPageOptions();
            return View(model);
        }

        [HttpGet("edit/{id}")]
        public IActionResult Edit(string id)
        {
            var model = _seoService.GetById(id);
            if (model == null) return NotFound();

            ViewBag.PageList = GetPageOptions();
            return View(model);
        }

        [HttpPost("edit/{id}")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(string id, PageSeo model, IFormFile bannerImage)
        {
            if (ModelState.IsValid)
            {
                var oldData = _seoService.GetById(id);
                if (bannerImage != null)
                {
                    string path = Path.Combine(_env.WebRootPath, "images/pages");
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);

                    string fileName = Guid.NewGuid() + Path.GetExtension(bannerImage.FileName);
                    string fullPath = Path.Combine(path, fileName);
                    using (var fs = new FileStream(fullPath, FileMode.Create))
                    {
                        bannerImage.CopyTo(fs);
                    }

                    model.psBanner = fileName;
                }
                else
                {
                    model.psBanner = oldData.psBanner;
                }
                model.psId = id;
                _seoService.Update(model);
                return RedirectToAction("Index");
            }

            ViewBag.PageList = GetPageOptions();
            return View(model);
        }

        [HttpGet("delete/{id}")]

        public IActionResult Delete(string id)
        {
            var model = _seoService.GetById(id);
            if(model != null)
            {
                _seoService.Delete(id);
            }
            return RedirectToAction("Index");
        }

      
    }
}
