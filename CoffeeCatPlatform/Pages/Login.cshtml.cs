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

        public LoginModel()
        {
            _customerRepo = new CustomerRepository();
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
    }
}
