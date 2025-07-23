using Microsoft.AspNetCore.Mvc;

namespace MakeMoreInternational.Areas.ho.Controllers
{
	[AuthorizeByCookie]
	[Area("ho")]
	
	public class DashboardController : Controller
	{
		[HttpGet]
		public IActionResult Index()
		{
			return View();
		}
	}
}
