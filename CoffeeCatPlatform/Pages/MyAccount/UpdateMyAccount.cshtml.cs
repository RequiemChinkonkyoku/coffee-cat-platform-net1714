using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DAOs;
using Models;
using Repositories;
using CoffeeCatPlatform.Pages.Shared;

namespace CoffeeCatPlatform.Pages.MyAccount
{
    public class UpdateMyAccountModel : CustomerAuthModel
    {
        private readonly IRepositoryBase<Customer> _customerRepo;

        public UpdateMyAccountModel(IRepositoryBase<Customer> customerRepo)
        {
            _customerRepo = customerRepo;
        }

        [BindProperty]
        public Customer Customer { get; set; } = default!;

        public IActionResult OnGet(int id)
        {
            IActionResult auth = CustomerAuthorize();
            if (auth != null)
            {
                return auth;
            }

            var customerId = HttpContext.Session.GetInt32("_Id");
            Customer = _customerRepo.GetAll().FirstOrDefault(p => p.CustomerId == customerId);
            return Page();

        }

        public IActionResult OnPost(int id)
        {

            if (!ModelState.IsValid)
            {
                // If the model state is not valid, return the page with validation errors
                return Page();
            }
            var customerId = HttpContext.Session.GetInt32("_Id");
            var existingCustomer = _customerRepo.GetAll().FirstOrDefault(p => p.CustomerId == customerId);

            if (existingCustomer == null)
            {
                TempData["ErrorMessage"] = "Customer not found.";
                return RedirectToPage("./ViewMyAccount");
            }

            existingCustomer.Name = Customer.Name;
            existingCustomer.Phone = Customer.Phone;
            existingCustomer.Email = Customer.Email;
            existingCustomer.Password = Customer.Password;

            _customerRepo.Update(existingCustomer);

            TempData["SuccessMessage"] = "Customer updated successfully.";
            return RedirectToPage("./ViewMyAccount");

        }
    }
}
