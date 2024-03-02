using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Repositories;

namespace CoffeeCatPlatform.Pages.CatManagement
{
    public class CatViewModel : PageModel
    {
        private readonly IRepositoryBase<Cat> _catRepository;

        public CatViewModel(IRepositoryBase<Cat> catRepository)
        {
            _catRepository = catRepository;
        }

        public List<Cat> Cats { get; set; }

        public IActionResult OnGet()
        {
            // Retrieve all cats from the repository for viewing
            Cats = _catRepository.GetAll();

            // Perform any additional operations if needed

            return Page();
        }
    }
}
