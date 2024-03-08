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
using Repositories.Impl;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Text.Json;

namespace CoffeeCatPlatform.Pages.CustomerPages.ReservationPages
{
    public class CreateReservationModel : PageModel
    {
        [BindProperty]
        public Reservation Reservation { get; set; } = default!;
        public List<Table> TableList { get; set; }
        public List<Reservation> ReservationList { get; set; }
        public List<ReservationTable> ReservationTables { get; set; }
        public List<Table> AvailableTables { get; set; }
        public List<Table> SelectedTables { get; set; }

        private readonly IRepositoryBase<Reservation> _reservationRepo;
        private readonly IRepositoryBase<Table> _tableRepo;
        private readonly IRepositoryBase<ReservationTable> _reservationTableRepo;


        private const string SessionKeyName = "_Name";
        private const string SessionKeyId = "_Id";
        private const string SessionKeyType = "_Type";

        public CreateReservationModel(IRepositoryBase<Reservation> reservationRepo,
                                      IRepositoryBase<Table> tableRepo,
                                      IRepositoryBase<ReservationTable> reservationTableRepo)
        {
            _reservationRepo = reservationRepo;
            _tableRepo = tableRepo;
            _reservationTableRepo = reservationTableRepo;

            TableList = new List<Table>();
            ReservationList = new List<Reservation>();
            ReservationTables = new List<ReservationTable>();
            AvailableTables = new List<Table>();
            SelectedTables = new List<Table>();
        }

        public IActionResult OnGet()
        {
            if (SessionCheck() == false)
            {
                return RedirectToPage("/ErrorPages/NotLoggedInError");
            }

            return Page();
        }

        public IActionResult OnPost(int? id)
        {
            if (_reservationRepo.GetAll() == null || Reservation == null)
            {
                return Page();
            }

            if (SessionCheck() == true)
            {
                Reservation.CustomerId = HttpContext.Session.GetInt32(SessionKeyId);
            }
            Reservation.Status = -1;
            _reservationRepo.Add(Reservation);

            if (Reservation.TotalPrice > (decimal)0.00)
            {
                TempData["ReservationID"] = Reservation.ReservationId;
                return RedirectToPage("/MomoPages/MomoInfo");
            }
            else
            {
                Reservation.Status = 1;
                _reservationRepo.Update(Reservation);
            }

            return RedirectToPage("/Homepage");
        }

        public IActionResult OnGetGetTable()
        {
            var result = JsonSerializer.Serialize(_tableRepo.GetAll(), new JsonSerializerOptions());

            return Content(result, "application/json");
        }

        private bool SessionCheck()
        {
            bool result = true;
            if (String.IsNullOrEmpty(HttpContext.Session.GetString(SessionKeyName))
                && String.IsNullOrEmpty(HttpContext.Session.GetString(SessionKeyId))
                && String.IsNullOrEmpty(HttpContext.Session.GetString(SessionKeyType)))
            {
                result = false;
            }
            return result;
        }
    }
}
