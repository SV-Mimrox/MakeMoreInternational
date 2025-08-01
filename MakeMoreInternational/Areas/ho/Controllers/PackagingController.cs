using MakeMoreInternational.Models;
using MakeMoreInternational.Services;
using Microsoft.AspNetCore.Mvc;

namespace MakeMoreInternational.Areas.ho.Controllers
{
	[Area("ho")]
	[Route("ho/packaging")]
	public class PackagingController : Controller
	{
		private readonly PackagingService _service;
		private readonly IWebHostEnvironment _env;

		public PackagingController(PackagingService service, IWebHostEnvironment env)
		{
			_service = service;
			_env = env;
		}
		[HttpGet("")]
		public IActionResult Index()
		{
			var data = _service.GetAll();
			return View(data);
		}

		[HttpGet("create")]
		public IActionResult Create() => View();

		[HttpPost("create")]
		public IActionResult Create(PackagingMaster cert, IFormFile pkgImageFile)
		{
			try
			{
				if (pkgImageFile != null)
				{
					string path = Path.Combine(_env.WebRootPath, "images/packaging");
					if (!Directory.Exists(path))
						Directory.CreateDirectory(path);

					string fileName = Guid.NewGuid() + Path.GetExtension(pkgImageFile.FileName);
					string fullPath = Path.Combine(path, fileName);
					using (var fs = new FileStream(fullPath, FileMode.Create))
					{
						pkgImageFile.CopyTo(fs);
					}

					cert.pkgImage = fileName;
				}

				_service.Create(cert);
				return RedirectToAction("Index");
			}
			catch (Exception ex)
			{
				return View(cert);
			}

		}

		[HttpGet("edit/{id}")]
		public IActionResult Edit(string id)
		{
			var data = _service.GetById(id);
			return View(data);
		}

		[HttpPost("edit/{id}")]
		public IActionResult Edit(string id, PackagingMaster cert, IFormFile pkgImageFile)
		{
			try
			{
				var existing = _service.GetById(id);

				if (pkgImageFile != null)
				{
					string path = Path.Combine(_env.WebRootPath, "images/packaging");
					string fileName = Guid.NewGuid() + Path.GetExtension(pkgImageFile.FileName);
					using (var fs = new FileStream(Path.Combine(path, fileName), FileMode.Create))
					{
						pkgImageFile.CopyTo(fs);
					}
					cert.pkgImage = fileName;
				}
				else
				{
					cert.pkgImage = existing.pkgImage;
				}

				


				cert.pkgId = id;
				_service.Update(id, cert);
				return RedirectToAction("Index");

			}
			catch (Exception ex)
			{
				return View(cert);
			}


		}

		[HttpGet("delete/{id}")]
		public IActionResult Delete(string id)
		{
			_service.Delete(id);
			return RedirectToAction("Index");
		}
	}
}
