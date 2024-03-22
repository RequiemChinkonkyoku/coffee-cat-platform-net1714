using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Repositories;

namespace CoffeeCatPlatform.Pages.CatManagement
{
    public class DeleteCatModel : PageModel
    {
        private readonly IRepositoryBase<Cat> _catRepository;

        public DeleteCatModel(IRepositoryBase<Cat> catRepository)
        {
            _catRepository = catRepository;
        }

        [BindProperty]
        public Cat Cat { get; set; }

        public IActionResult OnGet(int id)
        {
            Cat = _catRepository.GetAll().FirstOrDefault(c => c.CatId == id);

            if (Cat == null)
            {
                TempData["ErrorMessage"] = "Cat not found.";
                return RedirectToPage("./ViewCat");
            }

            return Page();
        }

        public IActionResult OnPost(int id)
        {
            var catToDelete = _catRepository.GetAll().FirstOrDefault(c => c.CatId == id);

            if (catToDelete == null)
            {
                TempData["ErrorMessage"] = "Cat not found.";
                return RedirectToPage("./ViewCat");
            }

            catToDelete.HealthStatus = 0;

            _catRepository.Update(catToDelete);

            TempData["SuccessMessage"] = "Cat deleted successfully.";
            return RedirectToPage("./ViewCat");
        }
    }
}

