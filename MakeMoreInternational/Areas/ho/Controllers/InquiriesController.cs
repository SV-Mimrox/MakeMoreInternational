using MakeMoreInternational.Services;
using Microsoft.AspNetCore.Mvc;

namespace MakeMoreInternational.Areas.ho.Controllers
{
    [Area("ho")]
    [Route("ho/inquiry")]
    public class InquiriesController : Controller
    {
        private readonly InquiryService _service;
        public InquiriesController(InquiryService service)
        {
            _service = service;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            var data = _service.GetAll().OrderByDescending(t => t.inqDate).ToList();
            return View(data);
        }


        [HttpGet("delete/{id}")]
        public IActionResult Delete(string id)
        {
            _service.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
