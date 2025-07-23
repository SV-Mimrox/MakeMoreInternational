using MakeMoreInternational.Services;
using Microsoft.AspNetCore.Mvc;

namespace MakeMoreInternational.Controllers
{
    public class InquiryController : BaseController
    {
        public InquiryController(WebSettingService service):base(service)
        {
            
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
