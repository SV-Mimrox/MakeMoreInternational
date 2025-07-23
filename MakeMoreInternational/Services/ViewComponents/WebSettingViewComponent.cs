using Microsoft.AspNetCore.Mvc;
using MakeMoreInternational.Services;
using MakeMoreInternational.Models;

namespace MakeMoreInternational.ViewComponents
{
    public class WebSettingViewComponent : ViewComponent
    {
        private readonly WebSettingService _service;

        public WebSettingViewComponent(WebSettingService service)
        {
            _service = service;
        }

        public IViewComponentResult Invoke()
        {
            var settings = _service.GetAll(); // You can also use .FirstOrDefault() for a single record
            return View(settings);
        }
    }
}
