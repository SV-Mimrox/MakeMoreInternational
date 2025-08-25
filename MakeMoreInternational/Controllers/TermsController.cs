using MakeMoreInternational.Services;
using Microsoft.AspNetCore.Mvc;

namespace MakeMoreInternational.Controllers
{
    public class TermsController : BaseController
    {
        private readonly WebPageService _service;
        public TermsController(WebSettingService service, CategoryService catService,
            PageSEOService seoService, LanguageService languageService, WebPageService tservice) : base(service, catService, seoService, languageService)
        {
            _service = tservice;
        }
        public IActionResult Index()
        {
            var data = _service.getTerms();
            return View(data);
        }
    }
}
