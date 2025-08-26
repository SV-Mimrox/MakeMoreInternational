using MakeMoreInternational.Models;
using MakeMoreInternational.Services;
using Microsoft.AspNetCore.Mvc;

namespace MakeMoreInternational.Areas.ho.Controllers
{
    [Area("ho")]
    [Route("ho/exhibition")]
	[AuthorizeByCookie]
	public class ExhibitionController : Controller
    {
        private readonly ExhibitionService _service;

        public ExhibitionController(ExhibitionService service)
        {
            _service = service;
        }

        private bool IsAuthenticated() => Request.Cookies["aid"] != null;

        [HttpGet("")]
        public IActionResult Index()
        {
            if (!IsAuthenticated()) return RedirectToAction("Index", "Login", new { area = "ho" });
            var data = _service.GetAll();
            return View(data);
        }

        [HttpGet("create")]
        public IActionResult Create()
        {
            if (!IsAuthenticated()) return RedirectToAction("Index", "Login", new { area = "ho" });
            return View();
        }

        [HttpPost("create")]
        public IActionResult Create(ExhibitionMaster model, IFormFile? ImageFile)
        {
            if (!IsAuthenticated()) return RedirectToAction("Index", "Login", new { area = "ho" });

            try
            {
                if (ImageFile != null && ImageFile.Length > 0)
                {
                    var fileName = Guid.NewGuid() + Path.GetExtension(ImageFile.FileName);
                    var folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/exhibitions");
                    if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

                    var filePath = Path.Combine(folder, fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                        ImageFile.CopyTo(stream);

                    model.ebmImage = fileName;
                }

                _service.Create(model);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }

    
        [HttpGet("delete/{id}")]
        public IActionResult Delete(string id)
        {
            if (!IsAuthenticated()) return RedirectToAction("Index", "Login", new { area = "ho" });

            var model = _service.GetById(id);
            if (model == null) return NotFound();

            if (!string.IsNullOrEmpty(model.ebmImage))
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/exhibitions", model.ebmImage);
                if (System.IO.File.Exists(path)) System.IO.File.Delete(path);
            }

            _service.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
