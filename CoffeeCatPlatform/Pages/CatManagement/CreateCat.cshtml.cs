using CoffeeCatPlatform.Pages.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Repositories;

namespace CoffeeCatPlatform.Pages.CatManagement
{
    public class CreateCatModel : ManagerAuthModel
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
            return Page();
        }
        public string DisplayGender
        {
            get { return Cat.Gender == 0 ? "Male" : "Female"; }
        }
        public IActionResult OnPost()
        {
            if (Cat.Birthday > DateTime.Today)
            {
                TempData["CatCreateErrorMessage"] = "Cat Birthday cannot further than current day.";
                return Page();
            }
            if (!ModelState.IsValid)
            {
                return Page();
            }
            Cat.HealthStatus = 1;
            Cat.ShopId = 1;
            _catRepository.Add(Cat);

            TempData["SuccessMessage"] = "Cat created successfully.";
            return RedirectToPage("/ManagerPages/CatManagement");
        }
    }
}
