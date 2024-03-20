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

namespace CoffeeCatPlatform.Pages.TableManagement
{
    public class UpdateTableModel : PageModel
    {
        private readonly IRepositoryBase<Table> _tableRepo;

        public UpdateTableModel(IRepositoryBase<Table> tableRepo)
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
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var existingTable = _tableRepo.GetAll().FirstOrDefault(t => t.TableId == id);

            if (existingTable == null)
            {
                TempData["ErrorMessage"] = "Table not found.";
                return RedirectToPage("./ViewTable");
            }
            existingTable.SeatCount = Table.SeatCount;
            existingTable.Status = Table.Status;
            existingTable.AreaId = Table.AreaId;

            _tableRepo.Update(existingTable);

            TempData["SuccessMessage"] = "Staff updated successfully.";
            return RedirectToPage("./ViewTable");
        }
    }
}
