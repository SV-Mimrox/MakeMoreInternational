using MakeMoreInternational.Services;
using Microsoft.AspNetCore.Mvc;

namespace MakeMoreInternational.Controllers
{
    public class PrivacyController : BaseController
    {
        private readonly WebPageService _service;
        public PrivacyController(WebSettingService service, CategoryService catService,
            PageSEOService seoService, LanguageService languageService, WebPageService tservice) : base(service, catService, seoService, languageService)
        {
            _service = tservice;
        }
        public IActionResult Index()
        {
            var data = _service.getPrivacy();
            return View(data);
        }
    }
}
