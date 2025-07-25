using MakeMoreInternational.Models;
using MakeMoreInternational.Models.custom;
using MakeMoreInternational.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace MakeMoreInternational.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly CategoryService _categoryService;
        private readonly ProductService _productService;
        private readonly WebSettingService _service;
        private SliderService _sliderService;
        private CertificateService _certiService;


        public HomeController(WebSettingService service,CategoryService categoryservice, ProductService productService,SliderService sliderService, CertificateService certiService)
            :base(service, categoryservice)
        {

            _sliderService = sliderService;
            _categoryService = categoryservice;
            _productService = productService;
            _certiService = certiService;
        }

        public IActionResult Index()
        {
            ViewBag.categories = _categoryService.GetHome().OrderBy(t=>t.seqNo);
            ViewBag.products = _productService.GetForHome().OrderBy(t => t.prdSeqNo);
            ViewBag.sliders = _sliderService.GetAll();
            ViewBag.certificates = _certiService.GetActive();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult ProductMenu()
        {
            var categories = _categoryService.GetAll(); // CategoryMaster
            var products = _productService.GetAll();    // ProductMaster

            var mainCats = categories.Where(c => string.IsNullOrEmpty(c.catParentId)).ToList();
            var subCats = categories.Where(c => !string.IsNullOrEmpty(c.catParentId)).ToList();

            var menu = mainCats.Select(cat => new ProductMenuVM
            {
                CategoryName = cat.catName,
                SubCategories = subCats
                    .Where(sub => sub.catParentId == cat.catId)
                    .Select(sub => new SubCategoryVM
                    {
                        SubCategoryName = sub.catName,
                        Products = products
                            .Where(p => p.catId == sub.catId)
                            .Select(p => new ProductVM
                            {
                                Name = p.prdName,
                                Slug = p.prdSlug,
                                Image = "/images/products/" + p.prdImage
                            }).ToList()
                    }).ToList()
            }).ToList();

            return PartialView("_ProductMenuPartial", menu);
        }

        
    }
}
