using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Repositories;
using Repositories.Impl;

namespace CoffeeCatPlatform.Pages.CatManagement
{
    public class InfoCatModel : PageModel
    {
        private readonly IRepositoryBase<Cat> _catRepository;

        public InfoCatModel(CatRepository catRepository)
        {
            _catRepository = catRepository;
        }

        [BindProperty]
        public Cat Cat { get; set; }

        public IActionResult OnGet(int? id)
        {
            // Retrieve the Cat from the repository based on the provided id
            var temp = _catRepository.GetAll().FirstOrDefault(c => c.CatId == id);

            if (temp == null)
            {
                TempData["ErrorMessage"] = "Cat not found.";
                return RedirectToPage("./ViewCat");
            }
            Cat = temp;

            return Page();
        }
    }
}
