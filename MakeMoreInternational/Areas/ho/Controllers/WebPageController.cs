using MakeMoreInternational.Models;
using MakeMoreInternational.Services;
using Microsoft.AspNetCore.Mvc;

namespace MakeMoreInternational.Areas.ho.Controllers
{
    [Area("ho")]
    [Route("ho/webpage")]
	[AuthorizeByCookie]
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
            var data = _service.getTerms();
            return View(data);
        }

        [HttpGet("privacy")]

        public IActionResult privacy()
        {
            var data = _service.getPrivacy();
            return View(data);
        }

        [HttpGet("infrastructure")]
        public IActionResult infrastructure()
        {
            var data = _service.getInfra();
            
            return View(data);
        }

        [HttpPost("save")]
        public IActionResult Save(string page, string pageData, string[] tcKeys, string[] tcValues, Terms terms,Infra infra, IFormFile[] infImages)
        {
            terms.tcUpdateDate = DateTime.Now;
            var dict = new Dictionary<string, string>();

            for (int i = 0; i < tcKeys.Length; i++)
            {
                if (!string.IsNullOrWhiteSpace(tcKeys[i]))
                {
                    dict[tcKeys[i]] = tcValues[i] ?? "";
                }
            }
            terms.tcDesc = dict;
            _service.savePage(page, pageData,terms,infra);
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

        [HttpPost("SaveInfra")]
        [ValidateAntiForgeryToken]
        public IActionResult SaveInfra(string page,string infraTitle,string infraDesc, IFormFile[] infImages)
        {
            Infra infra = new Infra();
            var oldData = _service.getInfra();
            List<string> images = new List<string>();
            if (oldData != null)
            {
                if (oldData.infraImages.Count >= 1)
                {
                    images.AddRange(oldData.infraImages);
                }
            }
            
            foreach(var image in infImages)
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(image.FileName);
                var folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/infrastructure");
                if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

                var filePath = Path.Combine(folder, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                    image.CopyTo(stream);

                images.Add(fileName);
            }
            
            infra.infraTitle = infraTitle;
            infra.infraDesc = infraDesc;
            infra.infraImages = images;
            _service.savePage(page, null, new Terms(), infra);
            return RedirectToAction("infrastructure");
        }

        [HttpGet("delete-infra-image/{id}")]
        public IActionResult DeleteInfraImage(string id)
        {
            _service.deleteInfraImage(id);
            return RedirectToAction("infrastructure");
        }
    }
}
