using MakeMoreInternational.Models;
using MakeMoreInternational.Services;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace MakeMoreInternational.Controllers
{
    public class HarvestChartController : BaseController
    {
        private readonly HarvestCategoryService _catSvc;
        private readonly HarvestProductService _prdSvc;
        private readonly HarvestSeasonService _sesSvc;

        [ActivatorUtilitiesConstructor]
        public HarvestChartController(
        WebSettingService webSettingService,  // << for BaseController
        HarvestCategoryService catSvc,
        HarvestProductService prdSvc,
        HarvestSeasonService sesSvc,
        CategoryService catService,
        PageSEOService seoService,
        LanguageService languageService)
        : base(webSettingService, catService, seoService, languageService)                    // pass up to Base
        {
            _catSvc = catSvc;
            _prdSvc = prdSvc;
            _sesSvc = sesSvc;
        }

        [HttpGet]
        [Route("harvest-chart/{id}")]
        public IActionResult Index(string id)
        {
            /* 1. category by slug */
            var cat = _catSvc.GetBySlug(id);
            if (cat == null) return NotFound();

            /* 2. products in that category */
            var products = _prdSvc.GetByCategory(cat.hcatId)
                                  .OrderBy(p => p.seqNo)
                                  .ToList();

            /* 3. all seasons for those products */
            var seasons = _sesSvc.GetAll()
                                 .Where(s => products.Any(p => p.hprdId == s.hprdId))
                                 .ToList();

            /* 4. build VM rows */
            var rows = products.Select(p => new HarvestSeasonRowVM
            {
                ProductName = p.hprdName,
                ProductImage=p.hprdIcon,
                Months = Enumerable.Range(1, 12)
                              .Select(m => seasons.Any(s => s.hprdId == p.hprdId
                                                         && s.month == m
                                                         && s.hsesStatus))
                              .ToArray()
            }).ToList();

            /* 5. pass to view */
            ViewBag.CategoryName = cat.hcatName;
            ViewBag.MonthNames = CultureInfo.CurrentCulture
                                   .DateTimeFormat.AbbreviatedMonthNames
                                   .Take(12).ToArray();
            return View(rows);
        }
    }
}
