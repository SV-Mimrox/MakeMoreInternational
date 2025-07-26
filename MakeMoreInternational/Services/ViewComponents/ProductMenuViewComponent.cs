using Microsoft.AspNetCore.Mvc;
using MakeMoreInternational.Services;
using System.Linq;

namespace MakeMoreInternational.ViewComponents
{
    public class ProductMenuViewComponent : ViewComponent
    {
        private readonly CategoryService _categoryService;
        private readonly ProductService _productService;

        public ProductMenuViewComponent(CategoryService categoryService, ProductService productService)
        {
            _categoryService = categoryService;
            _productService = productService;
        }

        public IViewComponentResult Invoke()
        {
            var categories = _categoryService.GetAll();
            var products = _productService.GetAll();

            var mainCats = categories.Where(c => string.IsNullOrEmpty(c.catParentId)).ToList();
            var subCats = categories.Where(c => !string.IsNullOrEmpty(c.catParentId)).ToList();

            var menu = mainCats.Select(cat => new Models.custom.ProductMenuVM
            {
                CategoryName = cat.catName,
                CategorySlug=cat.catSlug,
                SubCategories = subCats
                    .Where(sub => sub.catParentId == cat.catId)
                    .Select(sub => new Models.custom.SubCategoryVM
                    {
                        SubCategoryName = sub.catName,
                        Products = products
                            .Where(p => p.catId == sub.catId)
                            .Select(p => new Models.custom.ProductVM
                            {
                                Name = p.prdName,
                                Slug = p.prdSlug,
                                Image = "/images/products/" + p.prdIcon
                            }).ToList()
                    }).ToList()
            }).ToList();

            return View(menu);
        }
    }
}
