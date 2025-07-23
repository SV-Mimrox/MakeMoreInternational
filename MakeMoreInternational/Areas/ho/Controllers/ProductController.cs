using MakeMoreInternational.Models;
using MakeMoreInternational.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MakeMoreInternational.Controllers
{
    [Area("ho")]
    [Route("ho/product")]
    public class ProductController : Controller
    {
        private readonly ProductService _productService;
        private readonly CategoryService _categoryService;

        public ProductController(ProductService productService, CategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            var products = _productService.GetAll();
            return View(products);
        }

        [HttpGet("details/{id}")]
        public IActionResult Details(string id)
        {
            var product = _productService.GetById(id);
            if (product == null)
                return NotFound();
            return View(product);
        }

        [HttpGet("create")]
        public IActionResult Create()
        {
            ViewBag.Categories = _categoryService.GetAll();
            return View(new ProductMaster());
        }

        [HttpPost("create")]
        [RequestSizeLimit(104857600)]
        public IActionResult Create(ProductMaster product, IFormFile prdImageFile, IFormFile prdIconFile, IFormFile prdBannerFile)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = _categoryService.GetAll();
                return View(product);
            }

            // Check for duplicate seqNo within the same category
            var existingSeq = _productService.GetAll()
                .FirstOrDefault(p => p.catId == product.catId && p.prdSeqNo == product.prdSeqNo);

            if (existingSeq != null)
            {
                ModelState.AddModelError("seqNo", "This sequence number already exists for the selected category.");
                ViewBag.Categories = _categoryService.GetAll();
                return View(product);
            }

            // File Handling
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/products");
            if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);

            if (prdImageFile != null)
            {
                string img = Guid.NewGuid() + Path.GetExtension(prdImageFile.FileName);
                string path = Path.Combine(folderPath, img);
                using (var stream = new FileStream(path, FileMode.Create)) prdImageFile.CopyTo(stream);
                product.prdImage = img;
            }

            if (prdIconFile != null)
            {
                string icon = Guid.NewGuid() + Path.GetExtension(prdIconFile.FileName);
                string path = Path.Combine(folderPath, icon);
                using (var stream = new FileStream(path, FileMode.Create)) prdIconFile.CopyTo(stream);
                product.prdIcon = icon;
            }

            if (prdBannerFile != null)
            {
                string banner = Guid.NewGuid() + Path.GetExtension(prdBannerFile.FileName);
                string path = Path.Combine(folderPath, banner);
                using (var stream = new FileStream(path, FileMode.Create)) prdBannerFile.CopyTo(stream);
                product.prdBannerImage = banner;
            }

            // Slug
            if (string.IsNullOrWhiteSpace(product.prdSlug))
                product.prdSlug = product.prdName.Trim().ToLower().Replace(" ", "-");

            // Save
            product.createdAt = DateTime.Now;
            _productService.Create(product);

            return RedirectToAction("Index");
        }



        private string GenerateSlug(string name)
        {
            name = name.ToLowerInvariant().Trim();
            name = System.Text.RegularExpressions.Regex.Replace(name, @"[^a-z0-9\s-]", ""); // remove special chars
            name = System.Text.RegularExpressions.Regex.Replace(name, @"\s+", "-");         // replace space with dash
            return name;
        }


        [HttpGet("edit/{id}")]

        public IActionResult Edit(string id)
        {
            var product = _productService.GetById(id);
            if (product == null)
                return NotFound();

            ViewBag.CategoryList = _categoryService.GetAll()
    .Select(c => new SelectListItem
    {
        Value = c.catId,
        Text = c.catName
    })
    .ToList();
            return View(product);
        }

        [HttpPost("edit/{id}")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(
        string id,
        ProductMaster updatedProduct,
        IFormFile? prdImageFile,
        IFormFile? prdIconFile,
        IFormFile? prdBannerFile)
        {
            var existing = _productService.GetById(id);
            if (existing is null) return NotFound();

            /* ---------- 1. Unique sequence validation ---------- */
            bool duplicateSeq = _productService.GetAll()
                              .Any(p => p.catId == updatedProduct.catId
                                        && p.prdSeqNo == updatedProduct.prdSeqNo   // or prdSeqNo if that’s your prop
                                        && p.prdId != id);

            if (duplicateSeq)
                ModelState.AddModelError(nameof(updatedProduct.prdSeqNo),
                    "This sequence number already exists for the selected category.");

            if (!ModelState.IsValid)
            {
                ViewBag.Categories = _categoryService.GetAll();
                return View(updatedProduct);
            }

            /* ---------- 2. File handling  ---------- */
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/products");
            Directory.CreateDirectory(folderPath);

            // Banner
            if (prdBannerFile != null && prdBannerFile.Length > 0)
            {
                // delete old banner
                if (!string.IsNullOrEmpty(existing.prdBannerImage))
                {
                    var old = Path.Combine(folderPath, existing.prdBannerImage);
                    if (System.IO.File.Exists(old)) System.IO.File.Delete(old);
                }

                var bannerName = $"{Guid.NewGuid()}{Path.GetExtension(prdBannerFile.FileName)}";
                using var bs = System.IO.File.Create(Path.Combine(folderPath, bannerName));
                prdBannerFile.CopyTo(bs);
                updatedProduct.prdBannerImage = bannerName;
            }
            else
                updatedProduct.prdBannerImage = existing.prdBannerImage;

            // Image
            if (prdImageFile != null && prdImageFile.Length > 0)
            {
                if (!string.IsNullOrEmpty(existing.prdImage))
                {
                    var old = Path.Combine(folderPath, existing.prdImage);
                    if (System.IO.File.Exists(old)) System.IO.File.Delete(old);
                }

                var imgName = $"{Guid.NewGuid()}{Path.GetExtension(prdImageFile.FileName)}";
                using var istr = System.IO.File.Create(Path.Combine(folderPath, imgName));
                prdImageFile.CopyTo(istr);
                updatedProduct.prdImage = imgName;
            }
            else
                updatedProduct.prdImage = existing.prdImage;

            // Icon
            if (prdIconFile != null && prdIconFile.Length > 0)
            {
                if (!string.IsNullOrEmpty(existing.prdIcon))
                {
                    var old = Path.Combine(folderPath, existing.prdIcon);
                    if (System.IO.File.Exists(old)) System.IO.File.Delete(old);
                }

                var iconName = $"{Guid.NewGuid()}{Path.GetExtension(prdIconFile.FileName)}";
                using var istr = System.IO.File.Create(Path.Combine(folderPath, iconName));
                prdIconFile.CopyTo(istr);
                updatedProduct.prdIcon = iconName;
            }
            else
                updatedProduct.prdIcon = existing.prdIcon;

            /* ---------- 3. Preserve immutable / missing fields ---------- */
            updatedProduct.prdId = id;
            updatedProduct.createdAt = existing.createdAt;

            // If any dictionary came back null, keep originals
            updatedProduct.ProductDetails ??= existing.ProductDetails;
            updatedProduct.Varieties ??= existing.Varieties;
            updatedProduct.PhysicalSpecification ??= existing.PhysicalSpecification;
            updatedProduct.Grades ??= existing.Grades;
            updatedProduct.OtherDetails ??= existing.OtherDetails;

            /* ---------- 4. Slug regeneration (optional) ---------- */
            if (string.IsNullOrWhiteSpace(updatedProduct.prdSlug) ||
                !string.Equals(updatedProduct.prdName, existing.prdName, StringComparison.OrdinalIgnoreCase))
            {
                updatedProduct.prdSlug = GenerateSlug(updatedProduct.prdName);
            }

            /* ---------- 5. Persist ---------- */
            _productService.Update(id, updatedProduct);

            return RedirectToAction(nameof(Index));
        }


        [HttpGet("delete/{id}")]
        public IActionResult Delete(string id)
        {
            _productService.Delete(id);
            return RedirectToAction("Index");
        }

        private void HandleImageUpload(ProductMaster product, IFormFile prdImageFile, IFormFile prdIconFile)
        {
            // Upload Product Image
            if (prdImageFile != null && prdImageFile.Length > 0)
            {
                string imageName = Guid.NewGuid().ToString() + Path.GetExtension(prdImageFile.FileName);
                string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/products", imageName);
                Directory.CreateDirectory(Path.GetDirectoryName(imagePath));
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    prdImageFile.CopyTo(stream);
                }
                product.prdImage = imageName;
            }

            // Upload Product Icon
            if (prdIconFile != null && prdIconFile.Length > 0)
            {
                string iconName = Guid.NewGuid().ToString() + Path.GetExtension(prdIconFile.FileName);
                string iconPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/products/icons", iconName);
                Directory.CreateDirectory(Path.GetDirectoryName(iconPath));
                using (var stream = new FileStream(iconPath, FileMode.Create))
                {
                    prdIconFile.CopyTo(stream);
                }
                product.prdIcon = iconName;
            }
        }
    }
}
