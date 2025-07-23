using MakeMoreInternational.Models;
using MakeMoreInternational.Services;
using Microsoft.AspNetCore.Mvc;

namespace MakeMoreInternational.Areas.ho.Controllers
{
    [Area("ho")]
    [Route("ho/harvest-category")]
    public class HarvestCategoryController : Controller
    {
        private readonly HarvestCategoryService _svc;

        public HarvestCategoryController(HarvestCategoryService svc) => _svc = svc;

        private bool IsAuth() => Request.Cookies["aid"] != null;   // same pattern you used before

        /* ---------- Index ---------- */
        [HttpGet("")]
        public IActionResult Index()
        {
            if (!IsAuth()) return RedirectToAction("Index", "Login", new { area = "ho" });
            return View(_svc.GetAll());
        }

        /* ---------- Create ---------- */
        [HttpGet("create")]
        public IActionResult Create()
        {
            if (!IsAuth()) return RedirectToAction("Index", "Login", new { area = "ho" });
            return View(new HarvestCategoryMaster());
        }

        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public IActionResult Create(HarvestCategoryMaster model)
        {
            if (!IsAuth()) return RedirectToAction("Index", "Login", new { area = "ho" });

            if (!ModelState.IsValid) return View(model);

            model.hcatSlug = GenerateSlug(model.hcatName);

            _svc.Create(model);
            return RedirectToAction("Index");
        }

        /* ---------- Edit ---------- */
        [HttpGet("edit/{id}")]
        public IActionResult Edit(string id)
        {
            if (!IsAuth()) return RedirectToAction("Index", "Login", new { area = "ho" });

            var m = _svc.GetById(id);
            return m == null ? NotFound() : View(m);
        }

        [HttpPost("edit/{id}")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(string id, HarvestCategoryMaster model)
        {
            if (!IsAuth()) return RedirectToAction("Index", "Login", new { area = "ho" });
            if (!ModelState.IsValid) return View(model);

            var existing = _svc.GetById(id);
            if (existing == null) return NotFound();

            model.hcatId = id;
            model.createdAt = existing.createdAt;
            model.hcatSlug = GenerateSlug(model.hcatName); // always regenerate

            _svc.Update(id, model);
            return RedirectToAction("Index");
        }

        /* ---------- Delete ---------- */
        [HttpGet("delete/{id}")]
        public IActionResult Delete(string id)
        {
            if (!IsAuth()) return RedirectToAction("Index", "Login", new { area = "ho" });
            _svc.Delete(id);
            return RedirectToAction("Index");
        }

        private string GenerateSlug(string input)
        {
            return input?.Trim().ToLower().Replace(" ", "-").Replace("--", "-");
        }

    }
}
