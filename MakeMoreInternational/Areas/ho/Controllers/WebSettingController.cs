using MakeMoreInternational.Models;
using MakeMoreInternational.Services;
using Microsoft.AspNetCore.Mvc;

[AuthorizeByCookie]
[Route("ho/websetting")]
[Area("ho")]
public class WebSettingController : Controller
{
    private readonly WebSettingService _service;

    public WebSettingController(WebSettingService service)
    {
        _service = service;
    }

    // GET: ho/websetting
    [HttpGet("")]
    public IActionResult Index()
    {
        var setting = _service.GetSingle();
        if (setting == null)
        {
            var model = new WebSettingMaster();
            model.wsmSocialMedia.Add(new SocialMedia()); // at least one field
            return View("Edit", model);
        }

        return View("Edit", setting);
    }

    // POST: ho/websetting
    [HttpPost("edit")]
    public IActionResult Save(WebSettingMaster model, IFormFile? logoFile)
    {
        if (!ModelState.IsValid)
        {
            return View("Edit", model);
        }

        var existing = _service.GetSingle();

        // If logo uploaded
        if (logoFile != null && logoFile.Length > 0)
        {
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(logoFile.FileName);
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/logo");

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            var filePath = Path.Combine(folderPath, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                logoFile.CopyTo(stream);
            }

            // Delete old logo
            if (existing != null && !string.IsNullOrEmpty(existing.wsmLogo))
            {
                var oldPath = Path.Combine(folderPath, existing.wsmLogo);
                if (System.IO.File.Exists(oldPath))
                {
                    System.IO.File.Delete(oldPath);
                }
            }

            model.wsmLogo = fileName;
        }
        else if (existing != null)
        {
            model.wsmLogo = existing.wsmLogo; // retain old logo
        }

        _service.CreateOrUpdate(model);
        TempData["success"] = "Website settings saved successfully!";
        return RedirectToAction("Index");
    }
}
