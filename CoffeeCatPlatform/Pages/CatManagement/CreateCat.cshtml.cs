using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Repositories;
using System.Collections;

namespace CoffeeCatPlatform.Pages.CatManagement
{
    public class CreateCatModel : PageModel
    {
        private readonly IRepositoryBase<Cat> _catRepository;
        private readonly IRepositoryBase<AreaCat> _areacatRepository;
        private readonly IRepositoryBase<Area> _areaRepository;

        public CreateCatModel(IRepositoryBase<Cat> catRepository, IRepositoryBase<AreaCat> areacatRepository, IRepositoryBase<Area> areaRepository)
        {
            _catRepository = catRepository;
            _areacatRepository = areacatRepository;
            _areaRepository = areaRepository;
            AreaList = new List<Area>();
        }

        [BindProperty]
        public Cat Cat { get; set; }
        [BindProperty]
        public AreaCat AreaCat { get; set; }
        [BindProperty]
        public List<Area> AreaList { get; set; } = default!;

        public IActionResult OnGet()
        {
            AreaList = _areaRepository.GetAll();
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
            AreaCat.CatId = Cat.CatId;
            _areacatRepository.Add(AreaCat);

            TempData["SuccessMessage"] = "Cat created successfully.";
            return RedirectToPage("/ManagerPages/CatManagement");
        }
    }
}
