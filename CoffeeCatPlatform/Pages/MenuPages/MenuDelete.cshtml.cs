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

namespace CoffeeCatPlatform.Pages.MenuPages
{
    public class MenuDeleteModel : PageModel
    {
        private readonly IRepositoryBase<Product> _productRepo;

        public MenuDeleteModel(IRepositoryBase<Product> productRepo)
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

        public IActionResult OnPost(int id) { 
            var productToDelete = _productRepo.GetAll().FirstOrDefault(p =>p.ProductId == id);

            if (productToDelete == null) {
                TempData["ErrorMessage"] = "Product not found.";
                return RedirectToPage("./Menu");
            }

            productToDelete.productStatus = 0;

            _productRepo.Update(productToDelete);

            TempData["SuccessMessage"] = "Product deleted successfully.";
            return RedirectToPage("./Menu");
        }

        /*
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _productRepo.GetAll() == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FirstOrDefaultAsync(m => m.ProductId == id);

            if (product == null)
            {
                return NotFound();
            }
            else 
            {
                Product = product;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _productRepo.GetAll() == null)
            {
                return NotFound();
            }
            var product = await _context.Products.FindAsync(id);

            if (product != null)
            {
                Product = product;
                _productRepo.Delete(product);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
        */
    }
}
