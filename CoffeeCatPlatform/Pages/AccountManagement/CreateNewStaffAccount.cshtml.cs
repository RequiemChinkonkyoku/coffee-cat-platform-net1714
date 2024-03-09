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

namespace CoffeeCatPlatform.Pages.AccountManagement
{
    public class CreateNewStaffAccountModel : PageModel
    {
        private readonly IRepositoryBase<Staff> _staffRepo;

        public CreateNewStaffAccountModel(IRepositoryBase<Staff> staffRepo)
        {
            _staffRepo = staffRepo;
        }

        public IActionResult OnGet()
        {
        ViewData["RoleId"] = new SelectList(_staffRepo.GetAll(), "RoleId", "Name");
        ViewData["ShopId"] = new SelectList(_staffRepo.GetAll(), "ShopId", "ShopId");
            return Page();
        }

        [BindProperty]
        public Staff Staff { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _staffRepo.GetAll() == null || Staff == null)
            {
                return Page();
            }
            Staff.RoleId = 2;
            Staff.Status = 1;
            _staffRepo.Add(Staff);

            return RedirectToPage("./ViewAccount");
        }
    }
}
