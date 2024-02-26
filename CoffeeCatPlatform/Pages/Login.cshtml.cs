using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Repositories.Impl;
using Repositories;

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

        public LoginModel()
        {
            _customerRepo = new CustomerRepository();
            _staffRepo = new StaffRepository();
        }

        public void OnGet()
        {
        }
        public IActionResult OnPost()
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
                return RedirectToPage("/MenuPages/Menu", new { id = ID });
            }
        }
    }
}
