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
using CoffeeCatPlatform.Pages.Shared;

namespace CoffeeCatPlatform.Pages.AccountManagement
{
    public class StaffAccountUpdateModel : ManagerAuthModel
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
            if (existingStaff != null)
            {
                TempData["StaffUpdateErrorMessage"] = "This email address is already in use.";
                return Page();
            }

            Staff staff = _staffRepo.GetAll().FirstOrDefault(p => p.StaffId == id);
            staff.Name = Staff.Name;
            staff.Gender = Staff.Gender;
            staff.Phone = Staff.Phone;
            staff.Email = Staff.Email;
            staff.Password = Staff.Password;
            staff.Status = Staff.Status;
            staff.RoleId = Staff.RoleId;

            _staffRepo.Update(staff);

            TempData["SuccessMessage"] = "Staff updated successfully.";
            return RedirectToPage("/ManagerPages/StaffManagement");
        }
    }
}
