using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Repositories;
using Repositories.Impl;

namespace CoffeeCatPlatform.Pages.CatManagement
{
    public class EditCatModel : PageModel
    {
        private readonly IRepositoryBase<Cat> _catRepository;

        public EditCatModel(CatRepository catRepository)
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
                return RedirectToPage("/ManagerPages/CatManagement");
            }
            Cat = temp;

            return Page();
        }

        public IActionResult OnPostEdit(int id)
        {
            if (!ModelState.IsValid)
            {
                // If the model state is not valid, return the page with validation errors
                return Page();
            }

            // Retrieve the existing Cat from the repository based on the provided id
            //var existingCat = _catRepository.GetAll().FirstOrDefault(c => c.CatId == id);

            //if (existingCat == null)
            //{
            //    TempData["ErrorMessage"] = "Cat not found.";
            //    return RedirectToPage("./ViewCat");
            //}

            // Update the properties of the existingCat with the values from the posted Cat
/*            existingCat.Name = Cat.Name;
            existingCat.AreaCats = Cat.AreaCats;
            existingCat.HealthStatus = Cat.HealthStatus;
            existingCat.Breed = Cat.Breed;
            existingCat.Birthday = Cat.Birthday;
            existingCat.ImageUrl = Cat.ImageUrl;
            existingCat.Description = Cat.Description;*/

            // Update the existingCat in the repository
            _catRepository.Update(Cat);

            TempData["SuccessMessage"] = "Cat updated successfully.";
            return RedirectToPage("/ManagerPages/CatManagement");
        }

        public IActionResult OnPostDelete(int id)
        {
            var catToDelete = _catRepository.GetAll().FirstOrDefault(c => c.CatId == Cat.CatId);

            if (catToDelete == null)
            {
                TempData["ErrorMessage"] = "Cat not found.";
                return RedirectToPage("/ManagerPages/CatManagement");
            }

            // Update the HealthStatus to 0 (unhealthy)
            catToDelete.HealthStatus = 0;

            // Update the existing cat in the repository
            _catRepository.Update(catToDelete);

            TempData["SuccessMessage"] = "Cat deleted successfully.";
            return RedirectToPage("/ManagerPages/CatManagement");
        }
    }
}
