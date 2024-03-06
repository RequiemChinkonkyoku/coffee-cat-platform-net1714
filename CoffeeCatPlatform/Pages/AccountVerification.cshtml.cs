using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Repositories;
using Repositories.Impl;

namespace CoffeeCatPlatform.Pages
{
    public class AccountVerificationModel : PageModel
    {
        private readonly IRepositoryBase<Customer> _customerRepo;

        [BindProperty]
        public string Message { get; set; }

        public AccountVerificationModel()
        {
            _customerRepo = new CustomerRepository();
        }

        public void OnGet(string? _verificationToken)
        {
            for (int count = _customerRepo.GetAll().Count - 1; count >= 0; count--)
            {
                Customer customer = _customerRepo.GetAll()[count];
                string token = customer.CustomerId + customer.Email + customer.Password;
                string hashedToken = BCrypt.Net.BCrypt.HashString(token);
                if (BCrypt.Net.BCrypt.Verify(token, _verificationToken))
                {
                    if (customer.Status == 1)
                    {
                        Message = "Account is already verified.";
                    }
                    else
                    {
                        customer.Status = 1;
                        _customerRepo.Update(customer);
                        Message = "Account is verified. You now have access to more functions.";
                    }
                    break;
                }
            }
        }
    }
}
