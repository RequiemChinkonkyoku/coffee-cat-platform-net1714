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

namespace CoffeeCatPlatform.Pages.AccountManagement
{
    public class StaffAccountUpdateModel : PageModel
    {
        private readonly IRepositoryBase<Staff> _staffRepo;

        public StaffAccountUpdateModel(IRepositoryBase<Staff> staffRepo)
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
                return RedirectToPage("./ViewAccount");
            }
            return Page();
        }

        public IActionResult OnPost(int id)
        {

            if (!ModelState.IsValid)
            {
                return Page();
            }

            var existingStaff = _staffRepo.GetAll().FirstOrDefault(p => p.Email == Staff.Email && p.StaffId != id);
            if(existingStaff !=null)
            {
                TempData["StaffUpdateErrorMessage"] = "This email address is already in use.";
                return Page();
            }
            if (existingStaff == null)
            {
                TempData["ErrorMessage"] = "Staff not found.";
                return RedirectToPage("./ViewAccount");
            }
            existingStaff.Name = Staff.Name;
            existingStaff.Gender = Staff.Gender;
            existingStaff.Phone = Staff.Phone;
            existingStaff.Email = Staff.Email;
            existingStaff.Password = Staff.Password;
            existingStaff.Status = Staff.Status;
            existingStaff.RoleId = Staff.RoleId;

            _staffRepo.Update(existingStaff);

            TempData["SuccessMessage"] = "Staff updated successfully.";
            return RedirectToPage("./ViewAccount");

        }
    }
}
