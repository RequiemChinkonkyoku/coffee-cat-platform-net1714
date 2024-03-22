using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Repositories;
using Repositories.Impl;

namespace CoffeeCatPlatform.Pages
{
    public class LoginStaffModel : PageModel
    {
        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public string Password { get; set; }

        [BindProperty]
        public int ID { get; set; }

        [BindProperty]
        public string? Message { get; set; }

        private readonly IRepositoryBase<Customer> _customerRepo;
        private readonly IRepositoryBase<Staff> _staffRepo;

        private const string SessionKeyName = "_Name";
        private const string SessionKeyId = "_Id";
        private const string SessionKeyType = "_Type";

        public LoginStaffModel()
        {
            _customerRepo = new CustomerRepository();
            _staffRepo = new StaffRepository();
        }

        public void OnGet(string message)
        {
            Message = message;
        }

        public IActionResult OnPost()
        {
            string type1 = "Manager";
            string type2 = "Waiter";
            var staff = _staffRepo.GetAll().FirstOrDefault(c =>
                c.Email.Equals(Email) &&
                c.Password.Equals(Password));
            if (staff == null)
            {
                TempData["StaffLoginErrorMessage"] = "Invalid username or password.";
                return RedirectToPage("/LoginStaff");
            }
            else
            {
                if (SessionCheck() == false)
                {
                    HttpContext.Session.SetString(SessionKeyName, staff.Name);
                    HttpContext.Session.SetInt32(SessionKeyId, staff.StaffId);
                    if (staff.RoleId == 1)
                    {
                        HttpContext.Session.SetString(SessionKeyType, type1);
                        return RedirectToPage("/ManagerPages/Dashboard");
                    }
                    else if (staff.RoleId == 2)
                    {
                        HttpContext.Session.SetString(SessionKeyType, type2);
                        return RedirectToPage("/ManagerPages/Dashboard");
                    }
                }
                return RedirectToPage("/MenuPages/Menu", new { id = ID });
            }
        }

        private bool SessionCheck()
        {
            bool result = true;
            if (String.IsNullOrEmpty(HttpContext.Session.GetString(SessionKeyName))
                && String.IsNullOrEmpty(HttpContext.Session.GetString(SessionKeyId))
                && String.IsNullOrEmpty(HttpContext.Session.GetString(SessionKeyType)))
            {
                result = false;
            }
            return result;
        }
    }
}
