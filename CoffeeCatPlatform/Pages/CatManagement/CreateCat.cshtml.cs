using CoffeeCatPlatform.Pages.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Repositories;
using System.Collections;

namespace CoffeeCatPlatform.Pages.CatManagement
{
    public class CreateCatModel : ManagerAuthModel
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
            IActionResult auth = ManagerAuthorize();
            if (auth != null)
            {
                return auth;
            }

            AreaList =  _areaRepository.GetAll();

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
            AreaCat.CatId = Cat.CatId;
            AreaCat.AreaId = 1;
            _areacatRepository.Add(AreaCat);

            TempData["SuccessMessage"] = "Cat created successfully.";
            return RedirectToPage("/ManagerPages/CatManagement");
        }
    }
}
