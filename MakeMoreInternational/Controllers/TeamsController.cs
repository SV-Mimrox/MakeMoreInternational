using MakeMoreInternational.Services;
using Microsoft.AspNetCore.Mvc;

namespace MakeMoreInternational.Controllers
{
    public class TeamsController : BaseController
    {
        public TeamsController(WebSettingService service, CategoryService catService) : base(service, catService)
        {

        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
