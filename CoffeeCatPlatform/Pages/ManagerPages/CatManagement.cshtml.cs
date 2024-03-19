using CoffeeCatPlatform.Pages.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Repositories;

namespace CoffeeCatPlatform.Pages.ManagerPages
{
    public class CatManagementModel : ManagerAuthModel
    {
        private readonly IRepositoryBase<Cat> _catRepository;

        public CatManagementModel(IRepositoryBase<Cat> catRepository)
        {
            _catRepository = catRepository;
        }

        public IList<Cat> Cats { get; set; }

        public IActionResult OnGet()
        {
            IActionResult auth = ManagerAuthorize();
            if (auth != null)
            {
                return auth;
            }

            Cats = _catRepository.GetAll();
            return Page();
        }
    }
}
