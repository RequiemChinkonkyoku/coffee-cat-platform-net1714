using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DAOs;
using Models;
using Repositories;
using Repositories.Impl;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;

namespace CoffeeCatPlatform.Pages.MenuPages
{
    public class MenuModel : PageModel
    {
        private readonly IRepositoryBase<Product> _productRepo;

        public bool result = false;

        public MenuModel()
        {
            _productRepo = new ProductRepository();

        }
        [BindProperty(SupportsGet = true)]

        public int CurrentPage { get; set; } = 1;
        public int TotalItems { get; set; }
        public int ItemsPerPage { get; set; } = 4;


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

            return Page();
        }
    }
}
