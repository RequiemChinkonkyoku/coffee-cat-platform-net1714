using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DAOs;
using Models;
using Repositories;

namespace CoffeeCatPlatform.Pages.MenuPages
{
    public class MenuEditModel : PageModel
    {
        private readonly IRepositoryBase<Product> _productRepo;

        public MenuEditModel(IRepositoryBase<Product> productRepo)
        {
            _productRepo = productRepo;
        }

        [BindProperty]
        public Product Product { get; set; }


        public IActionResult OnGet(int id) {
            Product = _productRepo.GetAll().FirstOrDefault(p => p.ProductId == id);

            if (Product == null)
            {
                TempData["ErrorMessage"] = "Product not found.";
                return RedirectToPage("./Menu");
            }

            return Page();

        }

        public IActionResult OnPostEdit(int id) {
            if (!ModelState.IsValid)
            {
                // If the model state is not valid, return the page with validation errors
                return Page();
            }
            

            /*var existingProduct = _productRepo.GetAll().FirstOrDefault(p => p.ProductId == id);

            if (existingProduct == null)
            {
                TempData["ErrorMessage"] = "Product not found.";
                return RedirectToPage("./Menu");
            }

            existingProduct.Name = Product.Name;
            existingProduct.Description = Product.Description; 
            existingProduct.Price = Product.Price;
            existingProduct.Quantity = Product.Quantity;    
            existingProduct.ImageUrl = Product.ImageUrl;
            existingProduct.Shop = Product.Shop;*/
            Product.productStatus = 1;
            _productRepo.Update(Product);

            TempData["SuccessMessage"] = "Product updated successfully.";
            return RedirectToPage("./Menu");

        }

        public IActionResult OnPostDelete(int id)
        {
            var productToDelete = _productRepo.GetAll().FirstOrDefault(p => p.ProductId == Product.ProductId);

            if (productToDelete == null)
            {
                TempData["ErrorMessage"] = "Product not found.";
                return RedirectToPage("./Menu");
            }

            productToDelete.productStatus = 0;

            _productRepo.Update(productToDelete);

            TempData["SuccessMessage"] = "Product deleted successfully.";
            return RedirectToPage("./Menu");
        }
    }
}
