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

        /*
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _productRepo.GetAll() == null)
            {
                return NotFound();
            }

            var product =  await _context.Products.FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }
            Product = product;
           ViewData["ShopId"] = new SelectList(_context.Shops, "ShopId", "ShopId");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(Product.ProductId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }
        
        private bool ProductExists(int id)
        {
          return (_context.Products?.Any(e => e.ProductId == id)).GetValueOrDefault();
        }
        */

        public IActionResult OnGet(int id) {
            Product = _productRepo.GetAll().FirstOrDefault(p => p.ProductId == id);

            if (Product == null)
            {
                TempData["ErrorMessage"] = "Product not found.";
                return RedirectToPage("./Menu");
            }

            return Page();

        }

        public IActionResult OnPost(int id) 
        {

            if (!ModelState.IsValid)
            {
                // If the model state is not valid, return the page with validation errors
                return Page();
            }

            var existingProduct = _productRepo.GetAll().FirstOrDefault(p => p.ProductId == id);

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
            existingProduct.Shop = Product.Shop;

            _productRepo.Update(existingProduct);

            TempData["SuccessMessage"] = "Product updated successfully.";
            return RedirectToPage("./Menu");

        }
    }
}
