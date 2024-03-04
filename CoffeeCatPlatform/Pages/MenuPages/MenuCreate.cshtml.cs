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

namespace CoffeeCatPlatform.Pages.MenuPages
{
    public class MenuCreateModel : PageModel
    {
        private readonly IRepositoryBase<Product> _productRepo;

        public MenuCreateModel(IRepositoryBase<Product> productRepo)
        {
            _productRepo = productRepo;
        }

        public IActionResult OnGet()
        {
            ViewData["ShopId"] = new SelectList(_productRepo.GetAll(), "ShopId", "ShopId");
            return Page();
        }

        [BindProperty]
        public Product Product { get; set; } = default!;


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || _productRepo.GetAll() == null || Product == null)
            {
                return Page();
            }
            Product.productStatus = 1;
            _productRepo.Add(Product);

            return RedirectToPage("./Menu");
        }
    }
}
