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
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Microsoft.IdentityModel.Tokens;
using CoffeeCatPlatform.Pages.Shared;


namespace CoffeeCatPlatform.Pages.AccountManagement
{
    public class CreateNewStaffAccountModel : ManagerAuthModel
    {
        private readonly IRepositoryBase<Staff> _staffRepo;

        public CreateNewStaffAccountModel(IRepositoryBase<Staff> staffRepo)
        {
            _staffRepo = staffRepo;
        }

        private const string PhoneNumberPattern = @"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$";
        private const string EmailPattern = @"^([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5})$";
        private const string PasswordPattern = @"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[^\da-zA-Z])(?=.*\S).{8,}$";

        [BindProperty]
        public Staff Staff { get; set; }
        public string ErrorMessage { get; private set; }

        public IActionResult OnGet()
        {
            return Page();
        }

        public IActionResult OnPost()
        {
            if (string.IsNullOrEmpty(Staff.Name))
            {
                ErrorMessage = "Name is required.";
                return Page();
            }

            if (string.IsNullOrEmpty(Staff.Phone))
            {
                ErrorMessage = "Phone number is required.";
                return Page();
            }

            if (!Regex.IsMatch(Staff.Phone, @"^\d{10}$"))
            {
                ErrorMessage = "Please enter a valid 10-digit phone number.";
                return Page();
            }

            if (string.IsNullOrEmpty(Staff.Email))
            {
                ErrorMessage = "Email is required.";
                return Page();
            }

            if (!IsValidEmail(Staff.Email))
            {
                ErrorMessage = "Please enter a valid email address.";
                return Page();
            }

            if (_staffRepo.GetAll().Any(s => s.Email == Staff.Email))
            {
                ErrorMessage = "This email address is already in use.";
                return Page();
            }

            if (string.IsNullOrEmpty(Staff.Password))
            {
                ErrorMessage = "Password is required.";
                return Page();
            }

            if (Staff.Password.Length < 6)
            {
                ErrorMessage = "Password must be at least 6 characters long.";
                return Page();
            }

            Staff.RoleId = 2;
            Staff.Status = 1;
            _staffRepo.Add(Staff);

            return RedirectToPage("/ManagerPages/StaffManagement");
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
