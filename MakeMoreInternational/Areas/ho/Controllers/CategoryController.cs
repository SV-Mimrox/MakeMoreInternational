using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using MakeMoreInternational.Models;
using MakeMoreInternational.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MakeMoreInternational.Areas.ho.Controllers
{
    [Area("ho")]
    [Route("ho/category")]
    public class CategoryController : Controller
    {
        private readonly CategoryService _service;
        private readonly string _imageFolder =
            Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/categories");

        /* ----------------------------------------------------------- */
        public CategoryController(CategoryService service) => _service = service;

        private bool IsAuthenticated() => Request.Cookies["aid"] != null;

        /* ---------- slug helpers ----------------------------------- */
        private static string Slugify(string phrase)
        {
            string str = phrase.Trim().ToLowerInvariant();

            // Remove accents
            str = str.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();
            foreach (char ch in str)
            {
                var uc = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(ch);
                if (uc != System.Globalization.UnicodeCategory.NonSpacingMark)
                    sb.Append(ch);
            }

            str = sb.ToString();

            // Replace invalid chars with hyphens
            str = Regex.Replace(str, @"[^a-z0-9\s-]", "");
            str = Regex.Replace(str, @"\s+", " ").Trim();   // collapse whitespace
            str = str.Replace(" ", "-");
            str = Regex.Replace(str, @"-+", "-");           // collapse hyphens
            return str;
        }

        private string UniqueSlug(string baseSlug, string? excludeId = null)
        {
            string slug = baseSlug;
            int counter = 1;

            while (_service.GetAll()
                           .Any(c => c.catSlug == slug &&
                                     c.catId != excludeId))
            {
                slug = $"{baseSlug}-{counter++}";
            }
            return slug;
        }

        /* ---------- image helpers ---------------------------------- */
        private string SaveImage(IFormFile file)
        {
            if (!Directory.Exists(_imageFolder))
                Directory.CreateDirectory(_imageFolder);

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var path = Path.Combine(_imageFolder, fileName);

            using var stream = new FileStream(path, FileMode.Create);
            file.CopyTo(stream);

            return fileName;
        }

        private void DeleteImage(string? fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName)) return;

            var path = Path.Combine(_imageFolder, fileName);
            if (System.IO.File.Exists(path))
                System.IO.File.Delete(path);
        }

        /* ---------- LIST ------------------------------------------- */
        [HttpGet("")]
        public IActionResult Index()
        {
            if (!IsAuthenticated())
                return RedirectToAction("Index", "Login", new { area = "ho" });

            var data = _service.GetAll()
                               .OrderBy(c => c.catParentId)
                               .ThenBy(c => c.seqNo)
                               .ThenBy(c => c.catName)
                               .ToList();
            return View(data);
        }

        /* ---------- CREATE (GET) ----------------------------------- */
        [HttpGet("create")]
        public IActionResult Create()
        {
            if (!IsAuthenticated())
                return RedirectToAction("Index", "Login", new { area = "ho" });

            ViewBag.MainCategories = _service.GetMainCategories();
            return View(new CategoryMaster());
        }

        /* ---------- CREATE (POST) ---------------------------------- */
        [HttpPost("create")]
        public IActionResult Create(CategoryMaster model, IFormFile? catImageFile)
        {
            if (!IsAuthenticated())
                return RedirectToAction("Index", "Login", new { area = "ho" });

            try
            {
                /* --- SEQ NO logic (same as before) ------------------- */
                var siblings = _service.GetAll()
                                       .Where(c => c.catParentId == model.catParentId);

                if (model.seqNo <= 0)
                {
                    var maxSeq = siblings.DefaultIfEmpty().Max(c => c?.seqNo ?? 0);
                    model.seqNo = maxSeq + 1;
                }
                else if (siblings.Any(c => c.seqNo == model.seqNo))
                {
                    ModelState.AddModelError("seqNo",
                        "That Sequence No. is already used in this category level.");
                }

                /* --- SLUG logic -------------------------------------- */
                var baseSlug = Slugify(model.catName);
                model.catSlug = UniqueSlug(baseSlug);

                if (!ModelState.IsValid)
                {
                    ViewBag.MainCategories = _service.GetMainCategories();
                    return View(model);
                }

                if (catImageFile is { Length: > 0 })
                    model.catImage = SaveImage(catImageFile);

                _service.Create(model);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                ViewBag.MainCategories = _service.GetMainCategories();
                return View(model);
            }
        }

        /* ---------- EDIT (GET) ------------------------------------- */
        [HttpGet("edit/{id}")]
        public IActionResult Edit(string id)
        {
            if (!IsAuthenticated())
                return RedirectToAction("Index", "Login", new { area = "ho" });

            var model = _service.GetById(id);
            if (model == null) return NotFound();

            ViewBag.MainCategories = _service.GetMainCategories();
            return View(model);
        }

        /* ---------- EDIT (POST) ------------------------------------ */
        [HttpPost("edit/{id}")]
        public IActionResult Edit(string id, CategoryMaster model, IFormFile? catImageFile)
        {
            if (!IsAuthenticated())
                return RedirectToAction("Index", "Login", new { area = "ho" });

            try
            {
                var existing = _service.GetById(id);
                if (existing == null) return NotFound();

                /* Seq No. validation */
                if (model.seqNo <= 0)
                    ModelState.AddModelError("seqNo", "Sequence No. must be ≥ 1.");
                else if (existing.seqNo != model.seqNo &&
                         _service.GetAll()
                                 .Any(c => c.catParentId == model.catParentId &&
                                           c.seqNo == model.seqNo &&
                                           c.catId != id))
                {
                    ModelState.AddModelError("seqNo",
                        "That Sequence No. is already used in this category level.");
                }

                /* Slug regeneration */
                var baseSlug = Slugify(model.catName);
                model.catSlug = UniqueSlug(baseSlug, excludeId: id);

                if (!ModelState.IsValid)
                {
                    ViewBag.MainCategories = _service.GetMainCategories();
                    return View(model);
                }

                /* Image */
                if (catImageFile is { Length: > 0 })
                {
                    DeleteImage(existing.catImage);
                    model.catImage = SaveImage(catImageFile);
                }
                else
                {
                    model.catImage = existing.catImage;
                }

                _service.Update(id, model);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                ViewBag.MainCategories = _service.GetMainCategories();
                return View(model);
            }
        }

        /* ---------- DELETE ----------------------------------------- */
        [HttpGet("delete/{id}")]
        public IActionResult Delete(string id)
        {
            if (!IsAuthenticated())
                return RedirectToAction("Index", "Login", new { area = "ho" });

            var model = _service.GetById(id);
            if (model == null) return NotFound();

            DeleteImage(model.catImage);
            _service.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
