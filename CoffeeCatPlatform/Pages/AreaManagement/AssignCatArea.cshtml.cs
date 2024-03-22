using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DAOs;
using Models;
using Repositories;
using Repositories.Impl;

namespace CoffeeCatPlatform.Pages.AreaManagement
{
    public class AssignCatAreaModel : PageModel
    {
        private readonly IRepositoryBase<Cat> _catRepository;
        private readonly IRepositoryBase<AreaCat> _areacatRepository;
        private readonly IRepositoryBase<Area> _areaRepository;

        public AssignCatAreaModel(IRepositoryBase<AreaCat> areaCatRepo, IRepositoryBase<Cat> catRepo, IRepositoryBase<Area> areaRepository)
        {
            _areacatRepository = areaCatRepo;
            _catRepository = catRepo;
            _areaRepository = areaRepository;
            AreaList = new List<Area>();  
        }

        [BindProperty]
        public AreaCat AreaCat { get; set; } = default!;
        [BindProperty]
        public Cat Cat { get; set; } = default!;
        [BindProperty]
        public List<Area> AreaList { get; set; } = default!;

        public IActionResult OnGet(int id)
        {
            AreaCat = _areacatRepository.GetAll().FirstOrDefault(c => c.CatId == id);
            Cat = _catRepository.GetAll().FirstOrDefault(c => c.CatId == id);
            AreaList = _areaRepository.GetAll();

            if (AreaCat == null || Cat == null)
            {
                TempData["ErrorMessage"] = "Cat not found.";
                return RedirectToPage("./ViewArea");
            }

            return Page();
        }

        public IActionResult OnPost(int id)
        {
            var CatToAssign = _areacatRepository.GetAll().FirstOrDefault(c => c.CatId == id);

            if (CatToAssign == null)
            {
                TempData["ErrorMessage"] = "Cat not found.";
                return RedirectToPage("./ViewArea");
            }

            CatToAssign.AreaId = AreaCat.AreaId;

            _areacatRepository.Update(CatToAssign);

            TempData["SuccessMessage"] = "Cat assign to new area successfully.";
            return RedirectToPage("./ViewArea");
        }
    }
}
