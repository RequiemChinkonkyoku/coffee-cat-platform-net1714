using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Repositories.Impl;
using Repositories;
using Microsoft.IdentityModel.Tokens;
using CoffeeCatPlatform.Pages.Shared;

namespace CoffeeCatPlatform.Pages.ManagerPages
{
    public class StaffManagementModel : ManagerAuthModel
    {
        private readonly IRepositoryBase<Staff> _staffRepo;

        public bool result = false;

        public StaffManagementModel()
        {
            _staffRepo = new StaffRepository();

            StaffList = new List<Staff>();
        }
        public IList<Staff> StaffList { get; set; } = default!;

        public IActionResult OnGet()
        {
            IActionResult auth = ManagerAuthorize();
            if (auth != null)
            {
                return auth;
            }

            if (!_staffRepo.GetAll().IsNullOrEmpty())
            {
                if (HttpContext.Session.GetString("_Type").Equals("Admin"))
                {
                    var list = _staffRepo.GetAll();
                    foreach (var item in list)
                    {
                        StaffList.Add(item);
                    }
                    result = true;
                }
                else
                {
                    var list = _staffRepo.GetAll().Where(x => x.RoleId == 2);
                    foreach (var item in list)
                    {
                        StaffList.Add(item);
                    }
                    result = true;
                }
            }

            return Page();
        }
    }
}
