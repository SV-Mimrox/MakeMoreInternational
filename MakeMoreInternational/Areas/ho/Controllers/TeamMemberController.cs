using MakeMoreInternational.Models;
using Microsoft.AspNetCore.Mvc;

[Area("ho")]
[Route("ho/teams")]
public class TeamMemberController : Controller
{
    private readonly TeamMemberService _service;
    private readonly IWebHostEnvironment _env;

    public TeamMemberController(TeamMemberService service, IWebHostEnvironment env)
    {
        _service = service;
        _env = env;
    }

    [HttpGet("")]
    public IActionResult Index()
    {
        var data = _service.GetAll();
        return View(data);
    }

    [HttpGet("create")]
    public IActionResult Create()
    {
        return View(new TeamMemberMaster());
    }

    [HttpPost("create")]
    public IActionResult Create(TeamMemberMaster model, IFormFile PhotoFile)
    {
        if (ModelState.IsValid)
        {
            if (PhotoFile != null && PhotoFile.Length > 0)
            {
                string uploads = Path.Combine(_env.WebRootPath, "images/team");
                Directory.CreateDirectory(uploads);
                string fileName = Guid.NewGuid() + Path.GetExtension(PhotoFile.FileName);
                string path = Path.Combine(uploads, fileName);

                using (var fs = new FileStream(path, FileMode.Create))
                {
                    PhotoFile.CopyTo(fs);
                }

                model.Photo = fileName;
            }

            _service.Create(model);
            return RedirectToAction("Index");
        }
        return View(model);
    }

    [HttpGet("edit/{id}")]
    public IActionResult Edit(string id)
    {
        var model = _service.GetById(id);
        return View(model);
    }

    [HttpPost("edit/{id}")]
    public IActionResult Edit(string id, TeamMemberMaster model, IFormFile PhotoFile)
    {
        if (ModelState.IsValid)
        {
            var existing = _service.GetById(id);

            if (PhotoFile != null && PhotoFile.Length > 0)
            {
                string uploads = Path.Combine(_env.WebRootPath, "images/team");
                Directory.CreateDirectory(uploads);
                string fileName = Guid.NewGuid() + Path.GetExtension(PhotoFile.FileName);
                string path = Path.Combine(uploads, fileName);

                using (var fs = new FileStream(path, FileMode.Create))
                {
                    PhotoFile.CopyTo(fs);
                }

                model.Photo = fileName;
            }
            else
            {
                model.Photo = existing.Photo;
            }

            model.Id = id;
            _service.Update(id, model);
            return RedirectToAction("Index");
        }

        return View(model);
    }

    [HttpPost("delete/{id}")]
    public IActionResult Delete(string id)
    {
        _service.Delete(id);
        return RedirectToAction("Index");
    }

    [HttpGet("banner")]
    public IActionResult Banner()
    {
        var banner = _service.GetBanner() ?? new TeamMemberPageBanner();
        return View(banner);
    }

    [HttpPost("banner")]
    [ValidateAntiForgeryToken]
    public IActionResult Banner(IFormFile bannerFile)
    {
        if (bannerFile == null || bannerFile.Length == 0)
        {
            ModelState.AddModelError("BannerImage", "Please upload a banner image.");
            return View(_service.GetBanner() ?? new TeamMemberPageBanner());
        }

        _service.UpsertBanner(bannerFile);
        TempData["Success"] = "Banner updated successfully!";
        return RedirectToAction("banner");
    }
}
