using MakeMoreInternational.Services;
using Microsoft.AspNetCore.Mvc;

namespace MakeMoreInternational.Controllers
{
    public class InfoController : BaseController
    {

        private readonly ProductService _productService;


        public InfoController(ProductService productService, WebSettingService service) : base(service)
        {
            _productService = productService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("products/{id?}")]
        public IActionResult ProductDetails(string id)
        {
            var data = _productService.GetBySlug(id);
            return View(data);
        }
    }
}
