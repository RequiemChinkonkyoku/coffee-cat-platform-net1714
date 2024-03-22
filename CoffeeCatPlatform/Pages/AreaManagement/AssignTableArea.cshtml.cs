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
using System.Collections;

namespace CoffeeCatPlatform.Pages.AreaManagement
{
    public class AssignTableAreaModel : PageModel
    {
        private readonly IRepositoryBase<Table> _tableRepository;
        private readonly IRepositoryBase<Area> _areaRepository;

        public AssignTableAreaModel(IRepositoryBase<Table> tableRepo, IRepositoryBase<Area> areaRepository)
        {
            _tableRepository = tableRepo;
            _areaRepository = areaRepository;
            AreaList = new List<Area>();
        }

        [BindProperty]
        public Table Table { get; set; } = default!;

        [BindProperty]
        public List<Area> AreaList { get; set; } = default!;

        public IActionResult OnGet(int id)
        {
            Table = _tableRepository.GetAll().FirstOrDefault(t => t.TableId == id);
            AreaList = _areaRepository.GetAll();
            if (Table == null) 
            {
                TempData["ErrorMessage"] = "Table not found.";
                return RedirectToPage("./ViewArea");
            }
            return Page();
        }

        public IActionResult OnPost(int id)
        {

            var tableToAssign = _tableRepository.GetAll().FirstOrDefault(t => t.TableId == id);

            if (tableToAssign == null)
            {
                TempData["ErrorMessage"] = "Table not found.";
                return RedirectToPage("./ViewArea");
            }

            tableToAssign.AreaId = Table.AreaId;
            _tableRepository.Update(tableToAssign);

            TempData["SuccessMessage"] = "Table assign to new area successfully.";
            return RedirectToPage("./ViewArea");
        }
    }
}
