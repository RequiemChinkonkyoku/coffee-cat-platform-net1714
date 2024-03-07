using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Repositories;

namespace CoffeeCatPlatform.Pages.CatManagement
{
    public class CatViewModel : PageModel
    {
        private readonly IRepositoryBase<Cat> _catRepository;

        [BindProperty]
        public int NumberOfRows { get; set; }

        [BindProperty]
        public int NumberOfCats { get; set; }

        public CatViewModel(IRepositoryBase<Cat> catRepository)
        {
            _catRepository = catRepository;
        }

        public List<Cat> Cats { get; set; }

        public IActionResult OnGet()
        {
            // Retrieve all cats from the repository for viewing
            Cats = _catRepository.GetAll();

            double temp = Cats.Count;
            NumberOfCats = (int)temp;
            NumberOfRows = (int)Math.Ceiling(temp / 4);

            // Perform any additional operations if needed
            return Page();
        }
    }
}
