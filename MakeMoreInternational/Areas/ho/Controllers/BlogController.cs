using MakeMoreInternational.Models;
using MakeMoreInternational.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MakeMoreInternational.Areas.ho.Controllers
{
    [Area("ho")]
    [Route("ho/blog")]
    public class BlogController : Controller
    {
        private readonly BlogService _blogService;
        private readonly IWebHostEnvironment _env;

        public BlogController(BlogService blogService, IWebHostEnvironment env)
        {
            _blogService = blogService;
            _env = env;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            var blogs = _blogService.GetActive();
            return View(blogs);
        }

        [HttpGet("create")]
        public IActionResult Create()
        {
            return View(new BlogMaster());
        }

        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BlogMaster model, IFormFile blmImageFile)
        {
            try
            {
                if (blmImageFile != null && blmImageFile.Length > 0)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(blmImageFile.FileName);
                    var path = Path.Combine(_env.WebRootPath, "images/blog", fileName);

                    Directory.CreateDirectory(Path.GetDirectoryName(path));
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await blmImageFile.CopyToAsync(stream);
                    }

                    model.blmImage = fileName;
                }
                model.blmUrl = _blogService.GenerateUniqueSlug(model.blmTitle);
                model.blmDate = DateTime.Now;
                _blogService.Create(model);
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                return View(model);
            }
            
        }

        [HttpGet("edit/{id}")]
        public IActionResult Edit(string id)
        {
            var model = _blogService.GetById(id);
            return View(model);
        }

        [HttpPost("edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, BlogMaster model, IFormFile blmImageFile)
        {
            try
            {
                var existing = _blogService.GetById(id);

                if (blmImageFile != null && blmImageFile.Length > 0)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(blmImageFile.FileName);
                    var path = Path.Combine(_env.WebRootPath, "images/blog", fileName);

                    Directory.CreateDirectory(Path.GetDirectoryName(path));
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await blmImageFile.CopyToAsync(stream);
                    }

                    model.blmImage =  fileName;
                }
                else
                {
                    model.blmImage = existing.blmImage;
                }
                if (existing.blmTitle != model.blmTitle)
                {
                    model.blmUrl = _blogService.GenerateUniqueSlug(model.blmTitle);
                }
                else
                {
                    model.blmUrl = existing.blmUrl;
                }

                model.blmDate = existing.blmDate;
                model.blmId = id;
                _blogService.Update(id, model);
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                return View(model);
            }
            
        }

        [HttpGet("delete/{id}")]
        public IActionResult Delete(string id)
        {
            _blogService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
        
    }
}
