using MakeMoreInternational.Models;
using MakeMoreInternational.Services;
using Microsoft.AspNetCore.Mvc;

namespace MakeMoreInternational.Controllers
{
    public class InquiryController : BaseController
    {
        private readonly InquiryService _inqService;
        public InquiryController(WebSettingService service, CategoryService catService, InquiryService inqService) : base(service, catService)
        {
            _inqService = inqService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("/inquiry/submit")]
        public IActionResult Index(InquiryMaster inq)
        {
            try
            {
                _inqService.Create(inq);
                return Json("Success");
            }
            catch (Exception ex)
            {
                return Json("Error Occurred, Please try again");
            }
        }
    }
}
