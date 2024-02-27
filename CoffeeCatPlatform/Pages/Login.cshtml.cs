using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Repositories.Impl;
using Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using CoffeeCatPlatform.Pages.StaffPages;
using WebApplication1.Pages;

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

        private readonly IRepositoryBase<Customer> _customerRepo;
        private readonly IRepositoryBase<Staff> _staffRepo;

        private const string SessionKeyName = "_Name";
        private readonly ILogger<LoginModel> _logger;

        public LoginModel(ILogger<LoginModel> logger)
        {
            _customerRepo = new CustomerRepository();
            _staffRepo = new StaffRepository();
            _logger = logger;
        }

        public void OnGet()
        {

        }

        public IActionResult OnPostCustomer()
        {
            var customer = _customerRepo.GetAll().FirstOrDefault(c =>
                c.Email.Equals(Email) &&
                c.Password.Equals(Password));
            if (customer == null)
            {
                TempData["ErrorMessage"] = "Invalid username or password.";
                return RedirectToPage("/Login");
            }
            else
            {
                ID = customer.CustomerId;
                if (String.IsNullOrEmpty(HttpContext.Session.GetString(SessionKeyName)))
                {
                    HttpContext.Session.SetString(SessionKeyName, customer.Name);
                }
                _logger.LogInformation("Session Name: {Name}", customer.Name);
                return RedirectToPage("/MenuPages/Menu", new { id = ID });
            }
        }

        public IActionResult OnPostStaff()
        {
            var staff = _staffRepo.GetAll().FirstOrDefault(c =>
                c.Email.Equals(Email) &&
                c.Password.Equals(Password));
            if (staff == null)
            {
                TempData["ErrorMessage"] = "Invalid username or password.";
                return RedirectToPage("/Login");
            }
            else
            {
                ID = staff.StaffId;
                if (String.IsNullOrEmpty(HttpContext.Session.GetString(SessionKeyName)))
                {
                    HttpContext.Session.SetString(SessionKeyName, staff.Name);
                }
                ID = staff.StaffId;
                return RedirectToPage("/MenuPages/Menu", new { id = ID });
            }
        }
    }
}
