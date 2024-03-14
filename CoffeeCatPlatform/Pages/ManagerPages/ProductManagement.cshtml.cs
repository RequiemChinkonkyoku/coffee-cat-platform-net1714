using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Repositories.Impl;
using Repositories;

namespace CoffeeCatPlatform.Pages.ManagerPages
{
    public class ProductManagementModel : PageModel
    {
        private readonly IRepositoryBase<Product> _productRepo;
        private readonly IRepositoryBase<Category> _categoryRepo;

        public bool result = false;

        public ProductManagementModel()
        {
            _productRepo = new ProductRepository();
            _categoryRepo = new CategoryRepository();
        }

        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; } = 1;
        public int TotalItems { get; set; }
        public int ItemsPerPage { get; set; } = 5;

        public IList<Product> Products { get; set; } = default!;

        public int TotalPages => (int)Math.Ceiling((double)TotalItems / ItemsPerPage);

        public IActionResult OnGet(string? currentPage)
        {
            if (int.TryParse(currentPage, out int temp))
            {
                CurrentPage = temp;
            }
            else
            {
                CurrentPage = 1;
            }

            var allProducts = _productRepo.GetPaginated(CurrentPage, ItemsPerPage);

            TotalItems = _productRepo.GetAll().Count;

            Products = allProducts;

            result = Products.Count > 0;

            List<Category> categoryList = _categoryRepo.GetAll();
            foreach (var product in Products)
            {
                var category = categoryList.FirstOrDefault(x => x.CategoryId == product.CategoryId);
                if (category != null)
                {
                    product.Category = category;
                }
            }

            return Page();
        }
    }
}
