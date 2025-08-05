using MakeMoreInternational.Services;
using Microsoft.AspNetCore.Mvc;

namespace MakeMoreInternational.Areas.ho.Controllers
{
    [Area("ho")]
    [Route("ho/webpage")]
    public class WebPageController : Controller
    {
        private readonly WebPageService _service;
        private readonly IWebHostEnvironment _env;

        public WebPageController(WebPageService service, IWebHostEnvironment env)
        {
            _service = service;
            _env = env;
        }

        [HttpGet("terms")]
        public IActionResult terms()
        {
            var data = _service.getPageData("terms");
            ViewBag.data = data;
            return View();
        }

        [HttpGet("privacy")]

        public IActionResult privacy()
        {
            var data = _service.getPageData("privacy");
            ViewBag.data = data;
            return View();
        }

        [HttpGet("infrastructure")]
        public IActionResult infrastructure()
        {
            var data = _service.getPageData("infrastructure");
            ViewBag.data = data;
            return View();
        }

        [HttpPost("save")]
        public IActionResult Save(string page, string pageData)
        {
            _service.savePage(page, pageData);
            if(page == "terms")
            {
                return RedirectToAction("terms");
            }
            else if (page == "privacy")
            {
                return RedirectToAction("privacy");
            }
            else
            {
                return RedirectToAction("infrastructure");
            }
        }
    }
}
