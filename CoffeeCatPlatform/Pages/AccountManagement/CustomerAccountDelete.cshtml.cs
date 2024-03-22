using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DAOs;
using Models;
using Repositories;

namespace CoffeeCatPlatform.Pages.AccountManagement
{
    public class CustomerAccountDeleteModel : PageModel
    {
        private readonly IRepositoryBase<Customer> _customerRepo;

        public CustomerAccountDeleteModel(IRepositoryBase<Customer> customerRepo)
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
                ModelState.AddModelError("ErrorMessage", "Customer not found.");
                return RedirectToPage("./ViewAccount");
            }

            return Page();
        }

        public IActionResult OnPost(int id)
        {
            var customerToDelete = _customerRepo.GetAll().FirstOrDefault(p => p.CustomerId == id);

            if (customerToDelete == null)
            {
                ModelState.AddModelError("ErrorMessage", "Customer not found.");
                return RedirectToPage("./ViewAccount");
            }

            customerToDelete.Status = 0;

            _customerRepo.Update(customerToDelete);

            TempData["SuccessMessage"] = "Customer deleted successfully.";
            return RedirectToPage("./ViewAccount");
        }
    }
}
