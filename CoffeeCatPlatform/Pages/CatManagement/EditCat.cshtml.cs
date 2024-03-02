using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Repositories;

namespace CoffeeCatPlatform.Pages.CatManagement
{
    public class EditCatModel : PageModel
    {
        private readonly IRepositoryBase<Cat> _catRepository;

        public EditCatModel(IRepositoryBase<Cat> catRepository)
        {
            _catRepository = catRepository;
        }

        [BindProperty]
        public Cat Cat { get; set; }

        public IActionResult OnGet(int id)
        {
            // Retrieve the Cat from the repository based on the provided id
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
            if (!ModelState.IsValid)
            {
                // If the model state is not valid, return the page with validation errors
                return Page();
            }

            // Retrieve the existing Cat from the repository based on the provided id
            var existingCat = _catRepository.GetAll().FirstOrDefault(c => c.CatId == id);

            if (existingCat == null)
            {
                TempData["ErrorMessage"] = "Cat not found.";
                return RedirectToPage("./ViewCat");
            }

            // Update the properties of the existingCat with the values from the posted Cat
            existingCat.Name = Cat.Name;
            existingCat.AreaCats = Cat.AreaCats;
            existingCat.HealthStatus = Cat.HealthStatus;
            existingCat.Breed = Cat.Breed;
            existingCat.Birthday = Cat.Birthday;
            existingCat.ImageUrl = Cat.ImageUrl;

            // Update the existingCat in the repository
            _catRepository.Update(existingCat);

            TempData["SuccessMessage"] = "Cat updated successfully.";
            return RedirectToPage("./ViewCat");
        }
    }
}
