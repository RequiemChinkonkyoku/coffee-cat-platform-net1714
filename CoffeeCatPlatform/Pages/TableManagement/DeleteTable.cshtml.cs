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
using CoffeeCatPlatform.Pages.Shared;

namespace CoffeeCatPlatform.Pages.TableManagement
{
    public class DeleteTableModel : ManagerAuthModel
    {
        private readonly IRepositoryBase<Table> _tableRepo;

        public DeleteTableModel(IRepositoryBase<Table> tableRepo)
        {
            _tableRepo = tableRepo;
        }

        [BindProperty]
      public Table Table { get; set; } = default!;

        public IActionResult OnGet(int id)
        {
            Table = _tableRepo.GetAll().FirstOrDefault(t => t.TableId == id);

            if (Table == null) 
            {
                TempData["ErrorMessage"] = "Table not found.";
                return RedirectToPage("./ViewTable");
            }
            return Page();
        }

        public IActionResult OnPost(int id)
        {
            var tableToDelete = _tableRepo.GetAll().FirstOrDefault(t => t.TableId == id);

            if (tableToDelete == null) 
            {
                TempData["ErrorMessage"] = "Table not found.";
                return RedirectToPage("./ViewTable");
            }

            tableToDelete.Status = 0;
            _tableRepo.Update(tableToDelete);

            TempData["SuccessMessage"] = "Table deleted successfully.";
            return RedirectToPage("./ViewTable");
        }
    }
}
