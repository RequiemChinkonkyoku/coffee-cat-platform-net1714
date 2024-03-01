using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Repositories;
using Repositories.Impl;

namespace CoffeeCatPlatform.Pages
{
    public class RegisterModel : PageModel
    {
        [BindProperty]
        public Customer Customer { get; set; }

        private readonly IRepositoryBase<Customer> _customerRepo;

        private string _verificationToken;

        public RegisterModel()
        {
            _customerRepo = new CustomerRepository();
        }

        public void OnGet()
        {

        }

        public IActionResult OnPost()
        {
            if (_customerRepo.GetAll() == null || Customer == null)
            {
                return Page();
            }
            Customer.Status = 0;
            _customerRepo.Add(Customer);

            string token = Customer.Email + Customer.Password;
            _verificationToken = BCrypt.Net.BCrypt.HashString(token);
            return RedirectToPage("/Login");
        }
    }
}
