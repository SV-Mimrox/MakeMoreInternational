using MakeMoreInternational.Services;
using Microsoft.AspNetCore.Mvc;

namespace MakeMoreInternational.Controllers
{
    public class AboutController : BaseController
    {
        private readonly WebSettingService _service;
        private readonly AboutUsService _aboutUsService;
        

        [ActivatorUtilitiesConstructor]
        public AboutController(WebSettingService service, AboutUsService aboutUsService, 
            CategoryService catService, PageSEOService seoService, LanguageService languageService) : 
            base(service,catService, seoService, languageService)
        {
            _aboutUsService = aboutUsService;
            _service = service;
        }
       
        public IActionResult Index()
        {
            var data = _aboutUsService.Get();
            ViewBag.siteSetting = _service.GetAll().FirstOrDefault();
            ViewData["aboutData"] = data;
            return View();
        }
    }
}
