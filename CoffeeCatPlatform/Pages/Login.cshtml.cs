using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Repositories.Impl;
using Repositories;
using static System.Net.WebRequestMethods;

namespace CoffeeCatPlatform.Pages
{
    public class LoginModel : PageModel
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

        public LoginModel()
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
            string type = "Customer";
            var customer = _customerRepo.GetAll().FirstOrDefault(c =>
                c.Email.Equals(Email) &&
                c.Password.Equals(Password));

            if (customer == null)
            {
                TempData["LoginErrorMessage"] = "Invalid username or password.";
                return Page();
            }
            else
            {
                if (SessionCheck() == false)
                {
                    HttpContext.Session.SetString(SessionKeyName, customer.Name);
                    HttpContext.Session.SetInt32(SessionKeyId, customer.CustomerId);
                    HttpContext.Session.SetString(SessionKeyType, type);
                }
                return RedirectToPage("/");
            }
        }

        /*public IActionResult OnPostStaff()
        {
            string type1 = "Manager";
            string type2 = "Waiter";
            var staff = _staffRepo.GetAll().FirstOrDefault(c =>
                c.Email.Equals(Email) &&
                c.Password.Equals(Password));
            if (staff == null)
            {
                TempData["ErrorMessage"] = "Invalid username or password.";
                return Page();
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
                    }
                    else if (staff.RoleId == 2)
                    {
                        HttpContext.Session.SetString(SessionKeyType, type2);
                    }
                }
                return RedirectToPage("/MenuPages/Menu", new { id = ID });
            }
        }*/

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
