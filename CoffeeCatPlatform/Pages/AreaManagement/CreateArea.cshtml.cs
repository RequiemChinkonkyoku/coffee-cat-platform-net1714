using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DAOs;
using Models;
using Repositories;

namespace CoffeeCatPlatform.Pages.AreaManagement
{
    public class CreateAreaModel : PageModel
    {
        private readonly IRepositoryBase<Area> _areaRepo;

        [BindProperty]
        public Area Area { get; set; } = default!;
        public string ErrorMessage { get; private set; }
        public CreateAreaModel(IRepositoryBase<Area> areaRepo)
        {
            _areaRepo = areaRepo;
        }

        public IActionResult OnGet()
        {
            return Page();
        }
        public IActionResult OnPost()
        {
            if (String.IsNullOrEmpty(Area.Location))
            {
                ErrorMessage = "Area Location is required.";
                return Page();
            }
            Area.ShopId = 1;
            _areaRepo.Add(Area);
            return RedirectToPage("./ViewArea");
        }
    }
}
