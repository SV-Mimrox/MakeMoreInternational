using MakeMoreInternational.Services;
using Microsoft.AspNetCore.Mvc;

namespace MakeMoreInternational.Areas.ho.Controllers
{
    [Area("ho")]
    [Route("ho/contact")]
    public class ContactInquiryController : Controller
    {
        private readonly ContactService _contactService;
        public ContactInquiryController(ContactService contactService)
        {
            _contactService = contactService;
        }
        [HttpGet("")]
        public IActionResult Index()
        {
            var data = _contactService.GetAll().OrderByDescending(t=>t.cntDate).ToList();
            return View(data);
        }

        [HttpGet("delete/{id}")]
        public IActionResult Delete(string id)
        {
            _contactService.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
