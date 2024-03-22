using CoffeeCatPlatform.Pages.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;
using Models;
using Repositories;
using Repositories.Impl;

namespace CoffeeCatPlatform.Pages.ManagerPages
{
    public class StaffProfileModel : StaffAuthModel
    {
        private readonly IRepositoryBase<Staff> _staffRepo;

        [BindProperty]
        public Staff Staff { get; set; }

        private int? _id;

        public StaffProfileModel()
        {
            _staffRepo = new StaffRepository();
        }

        public IActionResult OnGet()
        {
            IActionResult auth = ManagerAuthorize();
            if (auth != null)
            {
                return auth;
            }

            _id = HttpContext.Session.GetInt32("_Id");

            if (!_staffRepo.GetAll().IsNullOrEmpty())
            {
                Staff = _staffRepo.GetAll().FirstOrDefault(x => x.StaffId == _id);
            }

            return Page();
        }
    }
}
