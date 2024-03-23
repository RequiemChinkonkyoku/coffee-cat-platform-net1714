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

namespace CoffeeCatPlatform.Pages.AccountManagement
{
    public class CustomerAccountUpdateModel : PageModel
    {
        private readonly IRepositoryBase<Customer> _customerRepo;

        public CustomerAccountUpdateModel(IRepositoryBase<Customer> customerRepo)
        {
            _customerRepo = customerRepo;
        }

        [BindProperty]
        public Customer Customer { get; set; } = default!;

        public IActionResult OnGet(int id)
        {
            Customer = _customerRepo.GetAll().FirstOrDefault(p => p.CustomerId == id);

            if (Customer == null)
            {
                TempData["ErrorMessage"] = "Customer not found.";
                return RedirectToPage("./ViewAccount");
            }

            return Page();

        }

        public IActionResult OnPost(int id)
        {

            if (!ModelState.IsValid)
            {
                return Page();
            }

            var existingCustomer = _customerRepo.GetAll().FirstOrDefault(p => p.CustomerId == id);

            if (existingCustomer == null)
            {
                TempData["ErrorMessage"] = "Customer not found.";
                return RedirectToPage("./ViewAccount");
            }

            existingCustomer.Name = Customer.Name;
            existingCustomer.Phone = Customer.Phone;
            existingCustomer.Email = Customer.Email;
            existingCustomer.Password = Customer.Password;
            existingCustomer.Status = Customer.Status;

            _customerRepo.Update(existingCustomer);

            TempData["SuccessMessage"] = "Customer updated successfully.";
            return RedirectToPage("./ViewAccount");

        }
    }
}
