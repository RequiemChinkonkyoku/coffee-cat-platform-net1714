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

        public AssignCatAreaModel(IRepositoryBase<AreaCat> areaCatRepo)
        {
            _areacatRepository = areaCatRepo;
        }

        [BindProperty]
        public AreaCat AreaCat { get; set; } = default!;
        public Cat Cat { get; set; }

        public IActionResult OnGet(int id)
        {
            AreaCat = _areacatRepository.GetAll().FirstOrDefault(c => c.CatId == id);

            if (AreaCat == null)
            {
                TempData["ErrorMessage"] = "Cat not found.";
                return RedirectToPage("/ViewArea");
            }

            return Page();
        }

        public IActionResult OnPost(int id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var CatToAssign = _areacatRepository.GetAll().FirstOrDefault(c => c.CatId == AreaCat.CatId);

            if (CatToAssign == null)
            {
                TempData["ErrorMessage"] = "Cat not found.";
                return RedirectToPage("/ViewArea");
            }
            CatToAssign.AreaId = AreaCat.AreaId;

            _areacatRepository.Update(CatToAssign);

            TempData["SuccessMessage"] = "Cat assign to new area successfully.";
            return RedirectToPage("./ViewArea");
        }
    }
}
