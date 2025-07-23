using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class AuthorizeByCookieAttribute : ActionFilterAttribute
{
	private readonly string _cookieName;

	public AuthorizeByCookieAttribute(string cookieName = "aid")
	{
		_cookieName = cookieName;
	}

	public override void OnActionExecuting(ActionExecutingContext context)
	{
		var cookie = context.HttpContext.Request.Cookies[_cookieName];
		if (string.IsNullOrEmpty(cookie))
		{
			// Redirect to login page if cookie not found
			context.Result = new RedirectToActionResult("Index", "Login", new { area = "ho" });
		}
	}
}
