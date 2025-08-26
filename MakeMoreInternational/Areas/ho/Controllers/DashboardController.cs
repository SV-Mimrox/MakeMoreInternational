using MakeMoreInternational.Models;
using MakeMoreInternational.Services;
using Microsoft.AspNetCore.Mvc;

namespace MakeMoreInternational.Areas.ho.Controllers
{
	[AuthorizeByCookie]
	[Area("ho")]

    public class DashboardController : Controller
	{
		private readonly CategoryService _service;
		private readonly ProductService _productService;
		private readonly HarvestCategoryService _hcatService;
		private readonly HarvestProductService _hprdService;
		private readonly HarvestSeasonService _seasonSvc;
		public DashboardController(CategoryService service, 
			ProductService productService, HarvestCategoryService hcatService, HarvestProductService hprdService, HarvestSeasonService seasonSvc)
        {
			_service = service;
			_productService = productService;
			_hcatService = hcatService;
			_hprdService = hprdService;
			_seasonSvc = seasonSvc;
		}

        [HttpGet]
		public IActionResult Index()
		{
			ViewBag.categoryCount = _service.GetAll().Count;
			ViewBag.productCount = _productService.GetAll().Count;	
			ViewBag.hcatCount = _hcatService.GetAll().Count;
			ViewBag.hprdCount = _hprdService.GetAll().Count;
			ViewBag.seasonCount = _seasonSvc.GetAll().Count;
			return View();
		}
	}
}
