using MakeMoreInternational.Services;
using Microsoft.AspNetCore.Mvc;

namespace MakeMoreInternational.Controllers
{
    public class TeamsController : BaseController
    {
        public TeamsController(WebSettingService service) : base(service)
        {

        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
