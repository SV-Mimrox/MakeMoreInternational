using MakeMoreInternational.Models;
using MakeMoreInternational.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;

namespace MakeMoreInternational.Controllers
{
    public class ContactController : BaseController
    {

        private readonly ContactService _contactService;
        public ContactController(WebSettingService service, CategoryService catService,
            ContactService contactService, PageSEOService seoService, LanguageService languageService) : 
            base(service, catService, seoService, languageService)
        {
            _contactService = contactService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("/contact/submit")]
        public IActionResult Index(ContactMaster cnt)
        {
            try
            {
                _contactService.Create(cnt);
                return Json("Success");
            }
            catch (Exception ex) {
                return Json("Error Occurred, Please try again");
            }
        }
    }
}
