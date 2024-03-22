using CoffeeCatPlatform.Pages.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Repositories;
using Repositories.Impl;

namespace CoffeeCatPlatform.Pages.MyAccount
{
    public class ViewMyAccountModel : CustomerAuthModel
    {
        private readonly IRepositoryBase<Customer> _customerRepo;

        public bool result = false;

        public ViewMyAccountModel()
        {
            _customerRepo = new CustomerRepository();
        }

        public Customer Customer { get; set; } = default!;

        public IActionResult OnGet()
        {
            IActionResult auth = CustomerAuthorize();
            if (auth != null)
            {
                return auth;
            }

            var id = HttpContext.Session.GetInt32("_Id");
            Customer = _customerRepo.GetAll().FirstOrDefault(x => x.CustomerId == id);
            result = true;

            return Page();
        }
    }
}
