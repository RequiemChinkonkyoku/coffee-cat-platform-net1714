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

namespace CoffeeCatPlatform.Pages.TableManagement
{
    public class CreateNewTableModel : PageModel
    {
        private readonly IRepositoryBase<Table> _tableRepo;

        public CreateNewTableModel(IRepositoryBase<Table> tableRepo)
        {
            _tableRepo = tableRepo;
        }

        [BindProperty]
        public Table Table { get; set; } = default!;
        public string ErrorMessage { get; private set; }

        public IActionResult OnGet()
        {
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
