using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DAOs;
using Models;
using Repositories;
using CoffeeCatPlatform.Pages.Shared;

namespace CoffeeCatPlatform.Pages.MenuPages
{
    public class MenuCreateModel : ManagerAuthModel
    {
        private readonly IRepositoryBase<Product> _productRepo;

        public MenuCreateModel(IRepositoryBase<Product> productRepo)
        {
            _productRepo = productRepo;
        }

        [BindProperty]
        public Product Product { get; set; }

        public IActionResult OnGet()
        {
            return Page();
        }

        public IActionResult OnPost()
        {
            if(ModelState.IsValid)
            {
                return Page();
            }
            Product.ShopId = 1;
            Product.productStatus = 1;
            _productRepo.Add(Product);

            TempData["SuccessMessage"] = "Product created successfully.";
            return RedirectToPage("/ManagerPages/ProductManagement");
        }
    }
}
