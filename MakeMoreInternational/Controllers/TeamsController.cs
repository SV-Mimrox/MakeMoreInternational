using MakeMoreInternational.Services;
using Microsoft.AspNetCore.Mvc;

namespace MakeMoreInternational.Controllers
{
    public class TeamsController : BaseController
    {
        private readonly TeamMemberService _service;
        public TeamsController(WebSettingService service, CategoryService catService,
            PageSEOService seoService, LanguageService languageService, TeamMemberService tservice) : base(service, catService, seoService, languageService)
        {
            _service = tservice;
        }
        public IActionResult Index()
        {
            ViewBag.members = _service.GetAll();
            return View();
        }
    }
}
