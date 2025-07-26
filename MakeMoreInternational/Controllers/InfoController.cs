using MakeMoreInternational.Services;
using Microsoft.AspNetCore.Mvc;

namespace MakeMoreInternational.Controllers
{
    public class InfoController : BaseController
    {

        private readonly ProductService _productService;
        private readonly CategoryService _categoryService;


        public InfoController(ProductService productService, WebSettingService service, CategoryService categoryService) : base(service, categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        [HttpGet("products")]
        public IActionResult Index()
        {
            
            var data = _productService.GetAllActive();
            return View(data);
        }

        [HttpGet("category/{id}")]
        public IActionResult category(string id)
        {
            var catData = _categoryService.GetBySlug(id);
            ViewBag.category = catData;
            var data = _productService.GetProductByCat(id);
            ViewBag.metaTitle = catData.catMetaTitle;
            ViewBag.metaKeyword = catData.catMetaKeywords;
            ViewBag.metaDesc = catData.catMetaDesc;
            ViewBag.metaIimage = catData.catImage;
            return View(data);
        }

        [HttpGet("products/{id?}")]
        public IActionResult ProductDetails(string id)
        {
            var data = _productService.GetBySlug(id);
            
            if (data != null)
            {
                ViewBag.metaTitle = data.prdMetaTitle;
                ViewBag.metaKeyword = data.prdMetaKeywords;
                ViewBag.metaDesc = data.prdMetaDesc;
                ViewBag.metaImage = data.prdImage;
                ViewBag.relatedProducts = _productService.GetRelatedProducts(data.catId).ToList();
            }
            return View(data);
        }
    }
}
