using MakeMoreInternational.Services;
using Microsoft.AspNetCore.Mvc;

namespace MakeMoreInternational.Controllers
{
    public class BlogsController : BaseController
    {
        
        public BlogsController(WebSettingService service) : base(service)
        {
            
        }
        public IActionResult Index()
        {
            
            return View();
        }
    }
}
