using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Repositories;

namespace CoffeeCatPlatform.Pages.ManagerPages
{
    public class CatManagementModel : PageModel
    {
        private readonly IRepositoryBase<Cat> _catRepository;

        public CatManagementModel(IRepositoryBase<Cat> catRepository)
        {
            _catRepository = catRepository;
        }

        public IList<Cat> Cats { get; set; }

        public IActionResult OnGet()
        {
            Cats = _catRepository.GetAll();
            return Page();
        }
    }
}
