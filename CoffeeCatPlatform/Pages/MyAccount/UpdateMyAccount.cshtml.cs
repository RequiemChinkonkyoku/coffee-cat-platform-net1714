﻿using System;
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

        public UpdateMyAccountModel(IRepositoryBase<Customer> customerRepo)
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
                return RedirectToPage("./ViewMyAccount");
            }

            return Page();

        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public IActionResult OnPost(int id)
        {

            if (!ModelState.IsValid)
            {
                // If the model state is not valid, return the page with validation errors
                return Page();
            }

            var existingCustomer = _customerRepo.GetAll().FirstOrDefault(p => p.CustomerId == id);

            if (existingCustomer == null)
            {
                TempData["ErrorMessage"] = "Customer not found.";
                return RedirectToPage("./ViewMyAccount");
            }

            existingCustomer.Name = Customer.Name;
            existingCustomer.Phone = Customer.Phone;
            existingCustomer.Email = Customer.Email;
            existingCustomer.Password = Customer.Password;
            existingCustomer.Status = Customer.Status;

            _customerRepo.Update(existingCustomer);

            TempData["SuccessMessage"] = "Customer updated successfully.";
            return RedirectToPage("./ViewMyAccount");

        }
    }
}
