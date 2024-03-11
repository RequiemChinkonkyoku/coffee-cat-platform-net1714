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

        [BindProperty]
        public Staff Staff { get; set; }

        public IActionResult OnGet()
        {
            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid) 
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
