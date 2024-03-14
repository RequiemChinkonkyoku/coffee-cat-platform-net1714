using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Repositories;
using Repositories.Impl;

namespace CoffeeCatPlatform.Pages.MyAccount
{
    public class ViewMyAccountModel : PageModel
    {
        private readonly IRepositoryBase<Customer> _customerRepo;
        private const string SessionKeyName = "_Name";
        private const string SessionKeyId = "_Id";
        private const string SessionKeyType = "_Type";

        public bool result = false;

        public ViewMyAccountModel()
        {
            _customerRepo = new CustomerRepository();
        }

        public Customer Customer { get; set; } = default!;

        public IActionResult OnGet()
        {
            if (SessionCheck() == false)
            {
                return RedirectToPage("/ErrorPages/NotLoggedInError");
            }
            var id = HttpContext.Session.GetInt32(SessionKeyId);
            Customer = _customerRepo.GetAll().FirstOrDefault(x => x.CustomerId == id);
            result = true;

            return Page();
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
