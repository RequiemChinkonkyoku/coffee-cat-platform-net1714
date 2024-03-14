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

namespace CoffeeCatPlatform.Pages.MyAccount
{
    public class UpdateMyAccountModel : PageModel
    {
        private readonly IRepositoryBase<Customer> _customerRepo;

        private const string SessionKeyName = "_Name";
        private const string SessionKeyId = "_Id";
        private const string SessionKeyType = "_Type";

        public UpdateMyAccountModel(IRepositoryBase<Customer> customerRepo)
        {
            _customerRepo = customerRepo;
        }

        [BindProperty]
        public Customer Customer { get; set; } = default!;

        public IActionResult OnGet(int id)
        {
            if (SessionCheck() == false)
            {
                return RedirectToPage("/ErrorPages/NotLoggedInError");
            }
            var customerId = HttpContext.Session.GetInt32(SessionKeyId);
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
            var customerId = HttpContext.Session.GetInt32(SessionKeyId);
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

        private bool SessionCheck()
        {
            bool result = true;
            if (String.IsNullOrEmpty(HttpContext.Session.GetString(SessionKeyName))
                && String.IsNullOrEmpty(HttpContext.Session.GetString(SessionKeyId))
                && String.IsNullOrEmpty(HttpContext.Session.GetString(SessionKeyType)))
            {
                result = false;
            }
            return result;
        }
    }
}
