using CoffeeCatPlatform.Pages.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;
using Models;
using Repositories;
using Repositories.Impl;

namespace CoffeeCatPlatform.Pages.ManagerPages
{
    public class AccountManagementModel : ManagerAuthModel
    {
        private readonly IRepositoryBase<Customer> _customerRepo;

        public bool result = false;

        [BindProperty]
        public int id { get; set; }

        public AccountManagementModel()
        {
            _customerRepo = new CustomerRepository();
        }

        public IList<Customer> Customers { get; set; } = default!;

        public IActionResult OnGet()
        {
            IActionResult auth = ManagerAuthorize();
            if (auth != null)
            {
                return auth;
            }

            if (!_customerRepo.GetAll().IsNullOrEmpty())
            {
                Customers = _customerRepo.GetAll();
                result = true;
            }
            return Page();
        }

        public IActionResult OnPostDeactivate()
        {
            Customer customer = _customerRepo.GetAll().FirstOrDefault(x => x.CustomerId == id);
            if (customer == null)
            {
                return Page();
            }
            customer.Status = 0;
            _customerRepo.Update(customer);
            return RedirectToPage();
        }
    }
}
