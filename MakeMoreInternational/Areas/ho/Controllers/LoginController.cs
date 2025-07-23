using MakeMoreInternational.Models;
using MakeMoreInternational.Services;
using Microsoft.AspNetCore.Mvc;

namespace MakeMoreInternational.Areas.ho.Controllers
{
    [Area("ho")]
	

	public class LoginController : Controller
    {
        private readonly AdminService _adminService;
        public LoginController(AdminService adminService)
        {
            _adminService = adminService;
        }
        public IActionResult Index()
        {
			//var data = new AdminMaster();
			//data.admEmail = "admin@gmail.com";
			//data.admContact = "9999999990";
			//data.admStatus = true;
			//data.admName = "Admin";
			//data.admPassword = "Admin@123";
			//_adminService.Register(data);
            return View();
        }

        [HttpPost]
		public IActionResult Index(string email,string password)
		{
            var data = _adminService.Login(email, password);
			if (data == null)
			{
				ViewBag.Message = "Invalid Email or Password";
				return View();
			}
			else
            {
				CookieOptions options = new CookieOptions
				{
					Expires = DateTimeOffset.UtcNow.AddDays(30), // persistent for 30 days
					HttpOnly = true,
					IsEssential = true
				};

				Response.Cookies.Append("aid", data.admId ?? "", options);
				return RedirectToAction("Index", "Dashboard", new {area="ho"});
			}
		}
	}
}
