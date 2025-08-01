using MakeMoreInternational.Models;
using MakeMoreInternational.Services;
using Microsoft.AspNetCore.Mvc;

namespace MakeMoreInternational.Areas.ho.Controllers
{
    [Area("ho")]
    [Route("ho/language")]
    public class LanguageController : Controller
    {
        private readonly LanguageService _service;

        public LanguageController(LanguageService service)
        {
            _service = service;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            var data = _service.GetAll();
            return View(data);
        }

        [HttpGet("create")]
        public IActionResult Create() => View(new LanguageMaster());

        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public IActionResult Create(LanguageMaster model)
        {
            if (!ModelState.IsValid)
                return View(model);

            _service.Create(model);
            return RedirectToAction("Index");
        }

        [HttpGet("edit/{id}")]
        public IActionResult Edit(string id)
        {
            var model = _service.GetById(id);
            return View(model);
        }

        [HttpPost("edit/{id}")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(string id, LanguageMaster model)
        {
            if (!ModelState.IsValid)
                return View(model);

            _service.Update(id, model);
            return RedirectToAction("Index");
        }

        [HttpGet("delete/{id}")]
        public IActionResult Delete(string id)
        {
            _service.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
