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

namespace CoffeeCatPlatform.Pages.AccountManagement
{
    public class StaffAccountDeleteModel : PageModel
    {
        private readonly IRepositoryBase<Staff> _staffRepo;

        public StaffAccountDeleteModel(IRepositoryBase<Staff> staffRepo)
        {
            _staffRepo = staffRepo;
        }

        [BindProperty]
      public Staff Staff { get; set; } = default!;

        public IActionResult OnGet(int id)
        {
            Staff = _staffRepo.GetAll().FirstOrDefault(p => p.StaffId == id);

            if (Staff == null)
            {
                TempData["ErrorMessage"] = "Staff not found.";
                return RedirectToPage("/ManagerPages/StaffManagement");
            }

            return Page();
        }

        public IActionResult OnPost(int id)
        {
            var staffToDelete = _staffRepo.GetAll().FirstOrDefault(p => p.StaffId == id);

            if (staffToDelete == null)
            {
                TempData["ErrorMessage"] = "Staff not found.";
                return RedirectToPage("/ManagerPages/StaffManagement");
            }

            staffToDelete.Status = 0;

            _staffRepo.Update(staffToDelete);

            TempData["SuccessMessage"] = "Customer deleted successfully.";
            return RedirectToPage("/ManagerPages/StaffManagement");
        }

    }
}
