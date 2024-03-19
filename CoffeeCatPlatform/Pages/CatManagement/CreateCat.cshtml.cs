using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Repositories;

namespace CoffeeCatPlatform.Pages.CatManagement
{
    public class CreateCatModel : PageModel
    {
        private readonly IRepositoryBase<Cat> _catRepository;

        public CreateCatModel(IRepositoryBase<Cat> catRepository)
        {
            _catRepository = catRepository;
        }

        [BindProperty]
        public Cat Cat { get; set; }

        public IActionResult OnGet()
        {
            // This is the handler for the GET request when loading the page
            return Page();
        }
        public string DisplayGender
        {
            get { return Cat.Gender == 0 ? "Male" : "Female"; }
        }
        public IActionResult OnPost()
        {
            // This is the handler for the POST request when submitting the form

            if (!ModelState.IsValid)
            {
                // If the model state is not valid, return the page with validation errors
                return Page();
            }
            Cat.HealthStatus = 1;
            Cat.ShopId = 1;
            // Add the new cat to the repository
            _catRepository.Add(Cat);

            TempData["SuccessMessage"] = "Cat created successfully.";
            return RedirectToPage("/ManagerPages/CatManagement");
        }
    }
}
