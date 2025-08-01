using MakeMoreInternational.Services;
using Microsoft.AspNetCore.Mvc;

namespace MakeMoreInternational.Controllers
{
    public class BlogsController : BaseController
    {
        private readonly BlogService _blogService;
        public BlogsController(WebSettingService service, CategoryService catService, 
            BlogService blogService, PageSEOService seoService, LanguageService languageService) 
            : base(service, catService, seoService, languageService)
        {
            _blogService = blogService;
        }
        public IActionResult Index()
        {
            var data = _blogService.GetActive();
            return View(data);
        }

        [HttpGet("/blog/{id}")]
        public IActionResult Details(string id)
        {
            var data = _blogService.GetByUrl(id);
            ViewBag.recent = _blogService.GetActive().OrderByDescending(t => t.blmDate).Take(5);
            ViewBag.metaTitle = data.blmMetaTitle;
            ViewBag.metaDesc = data.blmMetaDescription;
            ViewBag.metaKeyword = data.blmMetaKeywords;
            ViewBag.metaImage = data.blmImage;
            return View(data);
        }
    }
}
