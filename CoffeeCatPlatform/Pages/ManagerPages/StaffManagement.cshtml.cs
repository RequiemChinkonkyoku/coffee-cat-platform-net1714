using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Repositories.Impl;
using Repositories;
using Microsoft.IdentityModel.Tokens;

namespace CoffeeCatPlatform.Pages.ManagerPages
{
    public class StaffManagementModel : PageModel
    {
        private readonly IRepositoryBase<Staff> _staffRepo;

        public bool result = false;

        public StaffManagementModel()
        {
            _staffRepo = new StaffRepository();
        }
        public IList<Staff> StaffList { get; set; } = default!;

        public void OnGet()
        {
            if (!_staffRepo.GetAll().IsNullOrEmpty())
            {
                StaffList = _staffRepo.GetAll();
                result = true;
            }
        }
    }
}
