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

        public AssignCatAreaModel(AreaCatRepository areaCatRepository)
        {
            _areacatRepository = areaCatRepository;
        }

        [BindProperty]
        public AreaCat AreaCat { get; set; } = default!;
        public Cat Cat { get; set; }

        public IActionResult OnGet(int? id)
        {
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
                return Page();
            }

            var catoAssign = _areacatRepository.GetAll().FirstOrDefault(c => c.CatId == Cat.CatId);

            if (catoAssign == null)
            {
                TempData["ErrorMessage"] = "Cat not found.";
                return RedirectToPage("/ManagerPages/CatManagement");
            }
            catoAssign.AreaId = AreaCat.AreaId;

            return RedirectToPage("./Index");
        }

        
    }
}
