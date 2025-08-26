using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MakeMoreInternational.Models
{
    public class CheckCookieAttribute:ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var hasCookie = context.HttpContext.Request.Cookies.ContainsKey("aid");

            if (!hasCookie)
            {
                // Redirect to Login page
                context.Result = new RedirectToActionResult("Login", "Account", new { area = "ho" });
            }

            base.OnActionExecuting(context);
        }
    }
}
