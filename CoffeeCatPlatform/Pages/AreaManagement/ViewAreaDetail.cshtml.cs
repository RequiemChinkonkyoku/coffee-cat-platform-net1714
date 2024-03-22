using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DAOs;
using Models;
using Repositories;

namespace CoffeeCatPlatform.Pages.AreaManagement
{
    public class ViewAreaDetailModel : PageModel
    {
        private readonly IRepositoryBase<Area> _areaRepo;
        private readonly IRepositoryBase<AreaCat> _areacatRepo;
        private readonly IRepositoryBase<Table> _tableRepo;
        private readonly IRepositoryBase<Cat> _catRepo;

        [BindProperty]
        public Area Area { get; set; } = default!;
        [BindProperty]
        public List<AreaCat> AreaCat { get; set; } = default!;
        [BindProperty]
        public List<Table> Table { get; set; } = default!;
        [BindProperty]
        public List<Cat> Cat { get; set; }

        public ViewAreaDetailModel(IRepositoryBase<Area> areaRepo , IRepositoryBase<AreaCat> areacatRepo, IRepositoryBase<Table> tableRepo, IRepositoryBase<Cat> catRepo)
        {
            _areaRepo = areaRepo;
            _areacatRepo = areacatRepo;
            _tableRepo = tableRepo;
            _catRepo = catRepo;
            Cat = new List<Cat>();
            AreaCat = new List<AreaCat>();
            Table = new List<Table>();
        }

        public IActionResult OnGet(int? id)
        {
            var tempArea = _areaRepo.GetAll().FirstOrDefault(a => a.AreaId == id);
            var tempCatArea = _areacatRepo.GetAll().Where(a => a.AreaId == id);
            var tempTable = _tableRepo.GetAll().Where(t => t.AreaId == id);

            if (tempArea == null && tempCatArea == null && tempTable == null) 
            {
                TempData["ErrorMessage"] = "Area not found.";
                return RedirectToPage("./ViewArea");
            }
            Area = tempArea;

            foreach (var areaCat in tempCatArea)
            {
                var cat = _catRepo.FindById(areaCat.CatId.Value);
                Cat.Add(cat);
            }
            foreach (var tablearea in tempTable) 
            {
                var table = _tableRepo.FindById(tablearea.TableId);
                Table.Add(table);
            }

            return Page();
        }
    }
}