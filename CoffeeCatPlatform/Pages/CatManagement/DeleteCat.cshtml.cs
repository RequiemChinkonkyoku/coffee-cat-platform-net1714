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
                return RedirectToPage("./CatView");
            }

            return Page();
        }

        public IActionResult OnPost(int id)
        {
            var catToDelete = _catRepository.GetAll().FirstOrDefault(c => c.CatId == id);

            if (catToDelete == null)
            {
                TempData["ErrorMessage"] = "Cat not found.";
                return RedirectToPage("./CatView");
            }

            // Update the HealthStatus to 0 (unhealthy)
            catToDelete.HealthStatus = 0;

            // Update the existing cat in the repository
            _catRepository.Update(catToDelete);

            TempData["SuccessMessage"] = "Cat deleted successfully.";
            return RedirectToPage("./CatView");
        }
    }
}

