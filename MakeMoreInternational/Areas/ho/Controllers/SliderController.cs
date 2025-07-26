using MakeMoreInternational.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace MakeMoreInternational.Areas.ho.Controllers
{
    [Area("ho")]
    [Route("ho/slider")]
    public class SliderController : Controller
    {
        private readonly SliderService _sliderService;
        private readonly IWebHostEnvironment _env;
        public SliderController(SliderService sliderService, IWebHostEnvironment env)
        {
            _sliderService = sliderService;
            _env = env;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            var list = _sliderService.GetAll();
            return View(list);
        }

        [HttpGet("create")]
        public IActionResult Create() => View();

        [HttpPost("create")]
        public IActionResult Create(SliderMaster model, IFormFile mainImage, List<IFormFile> subImages)
        {
            string folderPath = Path.Combine(_env.WebRootPath, "images", "slider");
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            // Save main image
            if (mainImage != null && mainImage.Length > 0)
            {
                string mainFileName = Guid.NewGuid() + Path.GetExtension(mainImage.FileName);
                string mainPath = Path.Combine(folderPath, mainFileName);
                using (var stream = new FileStream(mainPath, FileMode.Create))
                {
                    mainImage.CopyTo(stream);
                }
                model.MainImage = mainFileName;
            }

            // Save extra images (max 7)
            if (subImages != null && subImages.Count > 0)
            {
                model.SubImages = new List<string>();
                int count = 0;

                foreach (var file in subImages)
                {
                    if (count >= 7) break;

                    if (file.Length > 0)
                    {
                        string fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                        string path = Path.Combine(folderPath, fileName);

                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            file.CopyTo(stream);
                        }

                        model.SubImages.Add(fileName);
                        count++;
                    }
                }
            }




            _sliderService.Create(model); // Save to MongoDB or your DB

            return RedirectToAction("Index");
        }

        [HttpGet("edit/{id}")]
        public IActionResult Edit(string id)
        {
            var slider = _sliderService.GetById(id);
            return View(slider);
        }

        [HttpPost("edit/{id}")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(string id, SliderMaster model, IFormFile? mainImageFile, List<IFormFile> newSubImages, string[] deleteSubImages)
        {
            if (ModelState.IsValid)
            {
                var existing = _sliderService.GetById(id);
                if (existing == null) return NotFound();

                // Create directory if not exists
                string uploadPath = Path.Combine(_env.WebRootPath, "images", "slider");
                if (!Directory.Exists(uploadPath))
                    Directory.CreateDirectory(uploadPath);

                // Main image replace
                if (mainImageFile != null)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(mainImageFile.FileName);
                    var filePath = Path.Combine(uploadPath, fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        mainImageFile.CopyTo(stream);
                    }

                    // Optionally delete old image
                    if (!string.IsNullOrEmpty(existing.MainImage))
                    {
                        var oldPath = Path.Combine(uploadPath, existing.MainImage);
                        if (System.IO.File.Exists(oldPath))
                            System.IO.File.Delete(oldPath);
                    }

                    model.MainImage = fileName;
                }
                else
                {
                    model.MainImage = existing.MainImage;
                }

                // Sub image handling
                var updatedSubImages = existing.SubImages.Where(img => !deleteSubImages.Contains(img)).ToList();

                // Save new sub-images
                if (newSubImages != null && newSubImages.Count > 0)
                {
                    foreach (var file in newSubImages)
                    {
                        if (updatedSubImages.Count >= 7)
                            break;

                        var subFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                        var subFilePath = Path.Combine(uploadPath, subFileName);
                        using (var stream = new FileStream(subFilePath, FileMode.Create))
                        {
                            file.CopyTo(stream);
                        }
                        updatedSubImages.Add(subFileName);
                    }
                }

                model.SubImages = updatedSubImages;
                model.Id = id;
                _sliderService.Update(id, model);

                return RedirectToAction("Index");
            }

            return View(model);
        }


        [HttpGet("delete/{id}")]
        public IActionResult Delete(string id)
        {
            _sliderService.Delete(id);
            return RedirectToAction("Index");
        }

    }
}
