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
        public List<int> SelectedTables { get; set; }

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
            SelectedTables = new List<int>();
        }

        public IActionResult OnGet()
        {
            if (SessionCheck() == false)
            {
                return RedirectToPage("/ErrorPages/NotLoggedInError");
            }

            var customerId = HttpContext.Session.GetInt32(SessionKeyId);

            var onGoingReservation = _reservationRepo.GetAll().Where(re => re.CustomerId == customerId && (re.Status == -1 || re.Status == 1));

            if (onGoingReservation.Any())
            {
                TempData["OnGoingReservation"] = onGoingReservation;
            }

            return Page();
        }

        public IActionResult OnPost()
        {
            if (Reservation == null)
            {
                return Page();
            }

            if (SessionCheck() == true)
            {
                Reservation.CustomerId = HttpContext.Session.GetInt32(SessionKeyId);
            }

            SelectedTables.Clear();

            foreach (var key in Request.Form.Keys)
            {
                if (key.StartsWith("selectedTable"))
                {
                    if (int.TryParse(Request.Form[key], out int tableId))
                    {
                        SelectedTables.Add(tableId);
                    }
                }
            }

            if (SelectedTables.Count > 0)
            {
                Reservation.Status = -1;
                _reservationRepo.Add(Reservation);

                foreach (var table in SelectedTables)
                {
                    var newReservationTable = new ReservationTable();
                    newReservationTable.ReservationId = Reservation.ReservationId;
                    newReservationTable.TableId = table;
                    _reservationTableRepo.Add(newReservationTable);
                }
            }
            else
            {
                return Page();
            }

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

        public IActionResult OnGetGetTable(DateTime bookingDay, TimeSpan startTime, TimeSpan endTime)
        {
            var tableList = CheckAvailableTable(bookingDay, startTime, endTime);

            var result = JsonSerializer.Serialize(tableList, new JsonSerializerOptions());

            return Content(result, "application/json");
        }

        private List<Table> CheckAvailableTable(DateTime bookingDate, TimeSpan startTime, TimeSpan endTime)
        {
            var availableTables = _tableRepo.GetAll();
            var reservationOfTheDay = new List<Reservation>();

            foreach (var reservation in _reservationRepo.GetAll())
            {
                if (reservation.BookingDay.Date.Equals(bookingDate))
                {
                    reservationOfTheDay.Add(reservation);
                }
            }

            if (reservationOfTheDay.Count != 0)
            {
                TableList = _tableRepo.GetAll();
                ReservationTables = _reservationTableRepo.GetAll();

                foreach (var reservation in reservationOfTheDay)
                {
                    var reservationStartTime = TimeSpan.Parse(reservation.StartTime.ToString());
                    var reservationEndTime = TimeSpan.Parse(reservation.EndTime.ToString());

                    var requestedStartTime = TimeSpan.Parse(startTime.ToString());
                    var requestedEndTime = TimeSpan.Parse(endTime.ToString());

                    if (!(requestedStartTime > reservationEndTime || requestedEndTime < reservationStartTime))
                    {
                        var overlapsedReservation = ReservationTables.Where(rt => rt.ReservationId == reservation.ReservationId);

                        foreach (var reservationTable in overlapsedReservation)
                        {
                            availableTables.RemoveAll(t => t.TableId == reservationTable.TableId);
                        }
                    }
                }
            }

            return availableTables;
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
