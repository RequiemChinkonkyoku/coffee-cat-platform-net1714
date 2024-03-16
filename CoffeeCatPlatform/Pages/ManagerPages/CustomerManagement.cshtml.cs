using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;
using Models;
using Repositories;
using Repositories.Impl;

namespace CoffeeCatPlatform.Pages.ManagerPages
{
    public class AccountManagementModel : PageModel
    {
        private readonly IRepositoryBase<Customer> _customerRepo;

        public bool result = false;

        public AccountManagementModel()
        {
            _customerRepo = new CustomerRepository();
        }

        public IList<Customer> Customers { get; set; } = default!;

        public void OnGet()
        {
            if (!_customerRepo.GetAll().IsNullOrEmpty())
            {
                Customers = _customerRepo.GetAll();
                result = true;
            }
        }
    }
}
