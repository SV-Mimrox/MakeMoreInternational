using MakeMoreInternational.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;

[Area("ho")]
[Route("ho/aboutus")]
public class AboutUsController : Controller
{
	private readonly AboutUsService _service;
	private readonly string _imgFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/aboutus");

	public AboutUsController(AboutUsService service)
	{
		_service = service;
	}

	private bool IsAuthenticated() => Request.Cookies["aid"] != null;

	private string SaveImage(IFormFile file)
	{
		if (!Directory.Exists(_imgFolder))
			Directory.CreateDirectory(_imgFolder);

		string fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
		string path = Path.Combine(_imgFolder, fileName);

		using var stream = new FileStream(path, FileMode.Create);
		file.CopyTo(stream);

		return fileName;
	}

	[HttpGet("")]
	public IActionResult Edit()
	{
		if (!IsAuthenticated())
			return RedirectToAction("Index", "Login", new { area = "ho" });

		var data = _service.Get() ?? new AboutUsMaster();
		return View(data);
	}

    [HttpPost("")]
    public IActionResult Edit(
      AboutUsMaster model,
      List<IFormFile> ValueImages,
      IFormFile MainImageFile,
      IFormFile WhyWeAreImageFile,
      IFormFile WhatWeDoImageFile)
    {
        var existing = _service.Get();
        // Save Main Image
        if (MainImageFile != null)
        {
            model.MainImage = SaveImage(MainImageFile);
        }
        else
        {
            model.MainImage = existing.MainImage;
        }

        // Save Why We Are Image
        if (WhyWeAreImageFile != null)
        {
            if (model.WhyWeAre == null)
                model.WhyWeAre = new AboutItem();
            model.WhyWeAre.Image = SaveImage(WhyWeAreImageFile);
        }
        else
        {
            model.WhyWeAre.Image = existing.WhyWeAre.Image;
        }
        // Save What We Do Image
        if (WhatWeDoImageFile != null)
        {
            if (model.WhatWeDo == null)
                model.WhatWeDo = new AboutItem();
            model.WhatWeDo.Image = SaveImage(WhatWeDoImageFile);
        }
        else
        {
            model.WhatWeDo.Image = existing.WhatWeDo.Image;
        }

        // Save Value Images
        if (model.Values != null && ValueImages != null)
        {
            for (int i = 0; i < model.Values.Count; i++)
            {
                if (i < ValueImages.Count && ValueImages[i] != null)
                {
                    model.Values[i].Image = SaveImage(ValueImages[i]);
                }
            }
        }

        // Fetch existing record (assuming single AboutUsMaster record)
        
        if (existing == null)
        {
            // Create new AboutUs entry
            _service.Create(model);
        }
        else
        {
            // Update existing
            model.Id = existing.Id;
            
            _service.Update(model.Id, model);
        }

        TempData["Success"] = "About Us section updated successfully.";
        return RedirectToAction("Edit");
    }

}
