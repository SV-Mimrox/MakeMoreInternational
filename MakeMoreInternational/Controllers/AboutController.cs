using MakeMoreInternational.Services;
using Microsoft.AspNetCore.Mvc;

namespace MakeMoreInternational.Controllers
{
    public class AboutController : BaseController
    {
        private readonly WebSettingService _service;
        private readonly AboutUsService _aboutUsService;

        [ActivatorUtilitiesConstructor]
        public AboutController(WebSettingService service, AboutUsService aboutUsService) : base(service)
        {
            _aboutUsService = aboutUsService;
        }
       
        public IActionResult Index()
        {
            var data = _aboutUsService.Get();
            ViewData["aboutData"] = data;
            return View();
        }
    }
}
