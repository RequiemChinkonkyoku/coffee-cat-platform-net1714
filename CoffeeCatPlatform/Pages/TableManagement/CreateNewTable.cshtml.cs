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
using System.Collections;
using CoffeeCatPlatform.Pages.Shared;

namespace CoffeeCatPlatform.Pages.TableManagement
{
    public class CreateNewTableModel : ManagerAuthModel
    {
        private readonly IRepositoryBase<Table> _tableRepo;
        private readonly IRepositoryBase<Area> _areaRepository;

        public CreateNewTableModel(IRepositoryBase<Table> tableRepo, IRepositoryBase<Area> areaRepository)
        {
            _tableRepo = tableRepo;
            _areaRepository = areaRepository;
            AreaList = new List<Area>();
        }

        [BindProperty]
        public Table Table { get; set; } = default!;
        [BindProperty]
        public List<Area> AreaList { get; set; } = default!;
        public string ErrorMessage { get; private set; }

        public IActionResult OnGet()
        {
            AreaList = _areaRepository.GetAll();
            return Page();
        }
        public IActionResult OnPost()
        {
          if (int.IsNegative(Table.SeatCount))
            {
                ErrorMessage = "Please Enter Valid Number.";
                return Page();
            }
            if (int.Equals(0 ,Table.SeatCount))
            {
                ErrorMessage = "Please Enter Valid Number.";
                return Page();
            }
            Table.Status = 1;
            _tableRepo.Add(Table);
            return RedirectToPage("./ViewTable");
        }
    }
}
