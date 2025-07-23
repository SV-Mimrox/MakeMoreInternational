using MakeMoreInternational.Services;
using Microsoft.AspNetCore.Mvc;

namespace MakeMoreInternational.Controllers
{
    public class ContactController : BaseController
    {
        public ContactController(WebSettingService service) : base(service)
        {
            
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
