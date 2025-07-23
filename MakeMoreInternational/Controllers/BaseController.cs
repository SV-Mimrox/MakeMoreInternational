using MakeMoreInternational.Services;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

public class BaseController : Controller
{
    private readonly WebSettingService _service;
    public BaseController(WebSettingService service)
    {
        _service = service;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        ViewBag.WebSetting = _service.GetAll().FirstOrDefault();
        base.OnActionExecuting(context);
    }
}
