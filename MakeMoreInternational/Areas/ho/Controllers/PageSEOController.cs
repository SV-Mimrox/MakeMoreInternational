using MakeMoreInternational.Models;
using MakeMoreInternational.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MongoDB.Bson;

namespace MakeMoreInternational.Areas.ho.Controllers
{
    [Area("ho")]
    [Route("ho/page-seo")]
    public class PageSEOController : Controller
    {
        private readonly PageSEOService _seoService;
        public PageSEOController(PageSEOService service)
        {
            _seoService = service;   
        }
        private List<SelectListItem> GetPageOptions()
        {
            return new List<SelectListItem>
        {
            new("Home", "home"),
            new("About", "about"),
            new("Team", "team"),
            new("Infrastructure", "infrastructure"),
            new("Harvest", "harvest"),
            new("Contact", "contact"),
            new("Product", "products"),
            new("Blog", "blog"),
            
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
            return View();
        }

        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PageSeo model)
        {
            if (ModelState.IsValid)
            {
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
        public IActionResult Edit(string id, PageSeo model)
        {
            if (ModelState.IsValid)
            {
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
            if (model == null) return NotFound();
            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(string id)
        {
            _seoService.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
