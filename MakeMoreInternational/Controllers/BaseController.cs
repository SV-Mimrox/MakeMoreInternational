using MakeMoreInternational.Services;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

public class BaseController : Controller
{
    private readonly WebSettingService _service;
    private readonly CategoryService _catService;
    private readonly PageSEOService _pageSEOService;
    private readonly LanguageService _languageService;
    public BaseController(WebSettingService service, CategoryService catService, PageSEOService pageSEOService, LanguageService languageService)
    {
        _service = service;
        _catService = catService;
        _pageSEOService = pageSEOService;
        _languageService = languageService;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        ViewBag.WebSetting = _service.GetAll().FirstOrDefault();
        ViewBag.footerCategories = _catService.GetHome();
        ViewBag.seoInfo = _pageSEOService.GetAll();
        ViewBag.languages = _languageService.GetActive();
        base.OnActionExecuting(context);
    }
}
