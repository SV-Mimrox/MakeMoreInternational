using MakeMoreInternational.Services;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

public class BaseController : Controller
{
    private readonly WebSettingService _service;
    private readonly CategoryService _catService;
    public BaseController(WebSettingService service, CategoryService catService)
    {
        _service = service;
        _catService = catService;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        ViewBag.WebSetting = _service.GetAll().FirstOrDefault();
        ViewBag.footerCategories = _catService.GetHome();
        base.OnActionExecuting(context);
    }
}
