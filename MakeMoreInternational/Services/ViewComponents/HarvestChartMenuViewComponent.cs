using Microsoft.AspNetCore.Mvc;

namespace MakeMoreInternational.Services.ViewComponents
{
    public class HarvestChartMenuViewComponent : ViewComponent
    {
        private readonly HarvestCategoryService _catSvc;

        public HarvestChartMenuViewComponent(HarvestCategoryService catSvc)
        {
            _catSvc = catSvc;
        }

        // invoked via  @await Component.InvokeAsync("HarvestChartMenu")
        public IViewComponentResult Invoke()
        {
            var cats = _catSvc.GetAll()
                                .Where(t=>t.hcatStatus==true)// or by name
                              .ToList();
            return View(cats);              // -> Default.cshtml
        }
    }
}
