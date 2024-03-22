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
    public class DeleteAreaModel : PageModel
    {
        private readonly IRepositoryBase<Area> _areaRepo;
        private readonly IRepositoryBase<AreaCat> _areacatRepository;
        private readonly IRepositoryBase<Table> _tableRepo;

        [BindProperty]
        public Area Area { get; set; } = default!;
        [BindProperty]
        public Table Table { get; set; } = default!;
        [BindProperty]
        public AreaCat AreaCat { get; set; } = default!;

        public DeleteAreaModel(IRepositoryBase<Area> areaRepo , IRepositoryBase<AreaCat> areacatRepository, IRepositoryBase<Table> tableRepo)
        {
            _areaRepo = areaRepo;
            _areacatRepository = areacatRepository;
            _tableRepo = tableRepo;
        }

        public IActionResult OnGet(int id)
        {
            Area = _areaRepo.GetAll().FirstOrDefault(a => a.AreaId == id);
            

            if (Area == null)
            {
                TempData["ErrorMessage"] = "Area not found.";
                return RedirectToPage("./ViewArea");
            }
            return Page();
        }

        public IActionResult OnPost(int id)
        {
            var AreaToDelete = _areaRepo.GetAll().FirstOrDefault(t => t.AreaId == id);
            var Table = _tableRepo.GetAll().Where(t => t.AreaId == Area.AreaId);
            var AreaCat = _areacatRepository.GetAll().Where(c => c.AreaId == Area.AreaId);

            if (Table.Count()>0 || AreaCat.Count()>0 )
            { 
                ModelState.AddModelError("ErrorMessage" , "Cannot Delete while there are cats and tables still in that area.") ;
                return Page();
            }

            _areaRepo.Delete(AreaToDelete);
            return RedirectToPage("./ViewArea");
        }
    }
}
