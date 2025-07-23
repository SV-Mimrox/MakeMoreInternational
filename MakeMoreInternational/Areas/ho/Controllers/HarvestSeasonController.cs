using MakeMoreInternational.Models;
using MakeMoreInternational.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Globalization;

namespace MakeMoreInternational.Areas.ho.Controllers
{
    [Area("ho")]
    [Route("ho/harvest-season")]
    public class HarvestSeasonController : Controller
    {
        private readonly HarvestSeasonService _seasonSvc;
        private readonly HarvestProductService _productSvc;

        public HarvestSeasonController(HarvestSeasonService s, HarvestProductService p)
        {
            _seasonSvc = s;
            _productSvc = p;
        }

        private bool IsAuth() => Request.Cookies["aid"] != null;
        private IActionResult LoginRedirect() =>
            RedirectToAction("Index", "Login", new { area = "ho" });

        private static string[] MonthNames() =>
            CultureInfo.CurrentCulture.DateTimeFormat.MonthNames.Take(12).ToArray();

        private void FillProductList()
        {
            ViewBag.ProductList = _productSvc.GetAll()
                .OrderBy(p => p.seqNo)
                .Select(p => new SelectListItem { Value = p.hprdId, Text = p.hprdName })
                .ToList();
        }

        /* ------------ LIST MATRIX ------------- */
        /* ---------- LIST (simple table view) ---------- */
        [HttpGet("")]
        public IActionResult Index()
        {
            if (!IsAuth()) return LoginRedirect();

            // 1.  full season list  (one document per product‑month)
            var seasons = _seasonSvc.GetAll();               // List<HarvestSeasonMaster>

            // 2.  dictionary  hprdId → ProductName  (for display in the view)
            var productDict = _productSvc.GetAll()
                             .ToDictionary(p => p.hprdId, p => p.hprdName);

            // 3.  pass dictionary via ViewBag
            ViewBag.ProductDict = productDict;

            // 4.  order rows: ProductName, then Month
            var ordered = seasons
                .OrderBy(s => productDict.ContainsKey(s.hprdId) ? productDict[s.hprdId] : "")
                .ThenBy(s => s.month)
                .ToList();

            return View(ordered);      // View expects: List<HarvestSeasonMaster>
        }


        /* ------------ SAVE MATRIX ------------- */
        [HttpPost("")]
        [ValidateAntiForgeryToken]
        public IActionResult Index(List<HarvestSeasonRowVM> rows)
        {
            if (!IsAuth()) return LoginRedirect();

            foreach (var row in rows)
            {
                var months = row.Months
                                .Select((val, idx) => (val, idx))
                                .Where(t => t.val)
                                .Select(t => t.idx + 1);
                _seasonSvc.ReplaceSeasonsForProduct(row.ProductId, months);
            }

            TempData["msg"] = "Harvest chart updated.";
            return RedirectToAction(nameof(Index));
        }

        /* ---------- PER‑PRODUCT CREATE ---------- */
        [HttpGet("create")]
        public IActionResult Create()
        {
            if (!IsAuth()) return LoginRedirect();
            FillProductList();
            ViewBag.MonthNames = MonthNames();
            return View(new HarvestSeasonMultiVM());   // Views/HarvestSeason/Create.cshtml
        }

        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public IActionResult Create(HarvestSeasonMultiVM vm)
        {
            if (!IsAuth()) return LoginRedirect();

            if (vm.Months == null || vm.Months.Length == 0)
                ModelState.AddModelError(nameof(vm.Months), "Select at least one month.");

            if (!ModelState.IsValid)
            {
                FillProductList();
                ViewBag.MonthNames = MonthNames();
                return View(vm);
            }

            _seasonSvc.ReplaceSeasonsForProduct(vm.ProductId, vm.Months);
            TempData["msg"] = "Months saved.";
            return RedirectToAction(nameof(Index));
        }

        /* ---------- PER‑PRODUCT EDIT ---------- */
        [HttpGet("edit/{productId}")]
        public IActionResult Edit(string productId)
        {
            if (!IsAuth()) return LoginRedirect();

            var product = _productSvc.GetById(productId);
            if (product == null) return NotFound();

            var months = _seasonSvc.GetAll()
                                   .Where(s => s.hprdId == productId)
                                   .Select(s => s.month)
                                   .ToArray();

            var vm = new HarvestSeasonMultiVM
            {
                ProductId = productId,
                Months = months,
                Status = true
            };

            ViewBag.ProductName = product.hprdName;
            ViewBag.MonthNames = MonthNames();
            return View(vm); // reuse Create.cshtml
        }

        [HttpPost("edit/{productId}")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(string productId, HarvestSeasonMultiVM vm)
        {
            if (!IsAuth()) return LoginRedirect();

            _seasonSvc.ReplaceSeasonsForProduct(productId, vm.Months ?? Array.Empty<int>());
            TempData["msg"] = "Months updated.";
            return RedirectToAction(nameof(Index));
        }
    }
}
