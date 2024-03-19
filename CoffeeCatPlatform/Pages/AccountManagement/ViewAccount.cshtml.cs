using CoffeeCatPlatform.Pages.MenuPages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using Models;
using Repositories;
using Repositories.Impl;

namespace CoffeeCatPlatform.Pages.AccountManagement
{
    public class ViewAccountModel : PageModel
    {
        private readonly IRepositoryBase<Staff> _staffRepo;

        private readonly IRepositoryBase<Customer> _customerRepo;

        public bool result = false;

        public ViewAccountModel() 
        {
            _staffRepo = new StaffRepository();
            _customerRepo = new CustomerRepository();
        }

        public IList<Staff> Staffs { get; set; } = default!;

        public IList<Customer> Customers { get; set; } = default!;

        public void OnGet()
        {
            if (!_staffRepo.GetAll().IsNullOrEmpty() || !_customerRepo.GetAll().IsNullOrEmpty()) 
            {
                Staffs = _staffRepo.GetAll();
                Customers = _customerRepo.GetAll();
                result = true;
            }
        }
    }
}
