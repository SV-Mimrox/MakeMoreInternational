using MakeMoreInternational.Models;
using MakeMoreInternational.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MakeMoreInternational.Areas.ho.Controllers
{
    [Area("ho")]
    [Route("ho/harvest-product")]
    public class HarvestProductController : Controller
    {
        private readonly HarvestProductService _svc;
        private readonly HarvestCategoryService _catSvc;

        public HarvestProductController(HarvestProductService svc, HarvestCategoryService catSvc)
        {
            _svc = svc;
            _catSvc = catSvc;
        }

        private bool IsAuth() => Request.Cookies["aid"] != null;   // same as other controllers

        /* ---------- Index ---------- */
        [HttpGet("")]
        public IActionResult Index()
        {
            if (!IsAuth()) return RedirectToAction("Index", "Login", new { area = "ho" });
            ViewData["CatDict"] = _catSvc.GetAll().ToDictionary(c => c.hcatId, c => c.hcatName);
            return View(_svc.GetAll().OrderBy(p => p.seqNo).ToList());
        }

        /* ---------- Create ---------- */
        [HttpGet("create")]
        public IActionResult Create()
        {
            if (!IsAuth()) return RedirectToAction("Index", "Login", new { area = "ho" });
            ViewBag.CategoryList = _catSvc.GetAll()
        .Select(c => new SelectListItem { Value = c.hcatId, Text = c.hcatName })
        .ToList();
            return View(new HarvestProductMaster());
        }

        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public IActionResult Create(HarvestProductMaster model, IFormFile? iconFile)
        {
            if (!IsAuth()) return RedirectToAction("Index", "Login", new { area = "ho" });
            ViewBag.CategoryList = _catSvc.GetAll()
        .Select(c => new SelectListItem { Value = c.hcatId, Text = c.hcatName })
        .ToList();

            if (_svc.SeqExists(model.hcatId, model.seqNo))
                ModelState.AddModelError(nameof(model.seqNo), "Sequence already used in this category.");

            if (!ModelState.IsValid) return View(model);

            SaveIcon(iconFile, ref model);

            _svc.Create(model);
            return RedirectToAction("Index");
        }

        /* ---------- Edit ---------- */
        [HttpGet("edit/{id}")]
        public IActionResult Edit(string id)
        {
            if (!IsAuth()) return RedirectToAction("Index", "Login", new { area = "ho" });
            ViewBag.CategoryList = _catSvc.GetAll()
        .Select(c => new SelectListItem { Value = c.hcatId, Text = c.hcatName })
        .ToList();
            var m = _svc.GetById(id);
            return m == null ? NotFound() : View(m);
        }

        [HttpPost("edit/{id}")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(string id, HarvestProductMaster model, IFormFile? iconFile)
        {
            if (!IsAuth()) return RedirectToAction("Index", "Login", new { area = "ho" });
            ViewBag.CategoryList = _catSvc.GetAll()
        .Select(c => new SelectListItem { Value = c.hcatId, Text = c.hcatName })
        .ToList();

            if (_svc.SeqExists(model.hcatId, model.seqNo, id))
                ModelState.AddModelError(nameof(model.seqNo), "Sequence already used in this category.");

            if (!ModelState.IsValid) return View(model);

            var existing = _svc.GetById(id);
            if (existing == null) return NotFound();

            model.hprdId = id;
            model.createdAt = existing.createdAt;
            model.hprdIcon = existing.hprdIcon;   // will be overwritten below if new icon

            SaveIcon(iconFile, ref model, existing.hprdIcon);

            _svc.Update(id, model);
            return RedirectToAction("Index");
        }

        /* ---------- Delete ---------- */
        [HttpGet("delete/{id}")]
        public IActionResult Delete(string id)
        {
            if (!IsAuth()) return RedirectToAction("Index", "Login", new { area = "ho" });

            var existing = _svc.GetById(id);
            if (existing != null && !string.IsNullOrEmpty(existing.hprdIcon))
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/harvest", existing.hprdIcon);
                if (System.IO.File.Exists(path)) System.IO.File.Delete(path);
            }
            _svc.Delete(id);
            return RedirectToAction("Index");
        }

        /* ---------- helper ---------- */
        private void SaveIcon(IFormFile? file, ref HarvestProductMaster model, string? oldFile = null)
        {
            if (file == null || file.Length == 0) return;

            var folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/harvest");
            Directory.CreateDirectory(folder);

            if (!string.IsNullOrEmpty(oldFile))
            {
                var oldPath = Path.Combine(folder, oldFile);
                if (System.IO.File.Exists(oldPath)) System.IO.File.Delete(oldPath);
            }

            var name = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            using var fs = System.IO.File.Create(Path.Combine(folder, name));
            file.CopyTo(fs);
            model.hprdIcon = name;
        }

        private void FillCategoryList() =>
    ViewBag.CategoryList = _catSvc.GetAll()
        .Select(c => new SelectListItem { Value = c.hcatId, Text = c.hcatName })
        .ToList();
    }
}
