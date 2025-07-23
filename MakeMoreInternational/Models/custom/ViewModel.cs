namespace MakeMoreInternational.Models.custom
{
    public class ViewModel
    {
    }
    public class ProductMenuVM
    {
        public string CategoryName { get; set; }
        public List<SubCategoryVM> SubCategories { get; set; } = new();
    }

    public class SubCategoryVM
    {
        public string SubCategoryName { get; set; }
        public List<ProductVM> Products { get; set; } = new();
    }

    public class ProductVM
    {
        public string Name { get; set; }
        public string Image { get; set; }
        public string Slug { get; set; }
    }

}
