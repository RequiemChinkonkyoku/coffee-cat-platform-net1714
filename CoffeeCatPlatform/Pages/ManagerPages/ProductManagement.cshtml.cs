using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Repositories.Impl;
using Repositories;
using CoffeeCatPlatform.Pages.Shared;

namespace CoffeeCatPlatform.Pages.ManagerPages
{
    public class ProductManagementModel : ManagerAuthModel
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
        public int ItemsPerPage { get; set; } = 10;

        [BindProperty(SupportsGet = true)]
        public string SearchQuery { get; set; }

        [BindProperty(SupportsGet = true)]
        public decimal MinPrice { get; set; }

        [BindProperty(SupportsGet = true)]
        public decimal MaxPrice { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SortByPrice { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SortByName { get; set; }

        public IList<Product> Products { get; set; } = default!;

        public int TotalPages => (int)Math.Ceiling((double)TotalItems / ItemsPerPage);

        public IActionResult OnGet(string? currentPage, string? searchQuery, decimal minPrice, decimal maxPrice, string sortByPrice, string sortByName)
        {
            IActionResult auth = ManagerAuthorize();
            if (auth != null)
            {
                return auth;
            }

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

            SearchQuery = searchQuery;
            MinPrice = minPrice;
            MaxPrice = maxPrice;
            SortByPrice = sortByPrice;
            SortByName = sortByName;

            IEnumerable<Product> query = _productRepo.GetAll();

            if (!string.IsNullOrEmpty(searchQuery))
            {
                query = query.Where(p => p.Name.Contains(searchQuery, StringComparison.OrdinalIgnoreCase));
            }

            if (MinPrice > 0 && MaxPrice > 0)
            {
                query = query.Where(p => p.Price >= MinPrice && p.Price <= MaxPrice);
            }

            if (sortByPrice == "asc")
            {
                query = query.OrderBy(p => p.Price);
            }
            else if (sortByPrice == "desc")
            {
                query = query.OrderByDescending(p => p.Price);
            }

            if (sortByName == "asc")
            {
                query = query.OrderBy(p => p.Name);
            }
            else if (sortByName == "desc")
            {
                query = query.OrderByDescending(p => p.Name);
            }

            TotalItems = query.Count();

            Products = query.Skip((CurrentPage - 1) * ItemsPerPage)
                            .Take(ItemsPerPage)
                            .ToList();

            return Page();
        }
    }
}
