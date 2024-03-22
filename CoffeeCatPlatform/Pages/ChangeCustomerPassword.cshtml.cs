using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Repositories.Impl;
using Repositories;

namespace CoffeeCatPlatform.Pages
{
    public class ChangeCustomerPasswordModel : PageModel
    {
        private readonly IRepositoryBase<Customer> _customerRepo;

        [BindProperty]
        public string Message { get; set; }

        [BindProperty]
        public string Password { get; set; }

        [BindProperty]
        public int Id { get; set; }

        public ChangeCustomerPasswordModel()
        {
            _customerRepo = new CustomerRepository();
        }

        public IActionResult OnGet(string? _verificationToken)
        {
            for (int count = _customerRepo.GetAll().Count - 1; count >= 0; count--)
            {
                Customer _customer = _customerRepo.GetAll()[count];
                string token = _customer.Email + DateTime.Today.ToString();
                string hashedToken = BCrypt.Net.BCrypt.HashString(token);
                if (BCrypt.Net.BCrypt.Verify(token, _verificationToken))
                {
                    Id = _customer.CustomerId;
                    return Page();
                }
            }
            return RedirectToPage("/Error404Page");
        }

        public IActionResult OnPost()
        {
            Customer customer = _customerRepo.GetAll().FirstOrDefault(x => x.CustomerId == Id);
            customer.Password = this.Password;
            _customerRepo.Update(customer);
            return RedirectToPage("/Login");
        }
    }
}
