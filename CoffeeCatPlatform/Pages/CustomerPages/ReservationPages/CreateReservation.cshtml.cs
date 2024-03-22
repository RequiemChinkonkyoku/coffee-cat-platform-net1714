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
using CoffeeCatPlatform.Pages.Shared;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;

namespace CoffeeCatPlatform.Pages.CustomerPages.ReservationPages
{
    public class CreateReservationModel : CustomerAuthModel
    {
        [BindProperty]
        public Reservation Reservation { get; set; } = default!;
        public List<Table> TableList { get; set; }
        public List<ReservationTable> ReservationTables { get; set; }
        public List<int> SelectedTables { get; set; }

        private readonly IRepositoryBase<Reservation> _reservationRepo;
        private readonly IRepositoryBase<Table> _tableRepo;
        private readonly IRepositoryBase<ReservationTable> _reservationTableRepo;
        private readonly IRepositoryBase<Area> _areaRepo;

        public CreateReservationModel(IRepositoryBase<Reservation> reservationRepo,
                                      IRepositoryBase<Table> tableRepo,
                                      IRepositoryBase<ReservationTable> reservationTableRepo,
                                      IRepositoryBase<Area> areaRepo)
        {
            _reservationRepo = reservationRepo;
            _tableRepo = tableRepo;
            _reservationTableRepo = reservationTableRepo;
            _areaRepo = areaRepo;

            TableList = new List<Table>();
            ReservationTables = new List<ReservationTable>();
            SelectedTables = new List<int>();
        }

        public IActionResult OnGet()
        {
            IActionResult auth = CustomerAuthorize();
            if (auth != null)
            {
                return auth;
            }

            var customerId = HttpContext.Session.GetInt32("_Id");

            var onGoingReservation = _reservationRepo.GetAll().Where(re => re.CustomerId == customerId
                                                                 && (re.Status == -1 || re.Status == 1));

            if (onGoingReservation.Any())
            {
                TempData["OnGoingReservation"] = onGoingReservation;
            }

            return Page();
        }

        public IActionResult OnPost()
        {
            IActionResult auth = CustomerAuthorize();
            if (auth != null)
            {
                return auth;
            }

            if (Reservation == null)
            {
                return Page();
            }

            if (Reservation.ArrivalDate.Date < DateTime.Now.Date)
            {
                ModelState.AddModelError("Invalid_ArrivalDate", "ArrivalDate cannot be before today");
                return Page();
            }

            if (Reservation.StartTime < DateTime.Now.TimeOfDay && Reservation.ArrivalDate.Date == DateTime.Now.Date)
            {
                ModelState.AddModelError("Invalid_StartTime", "StartTime cannot be earlier than current time");
                return Page();
            }

            if (Reservation.EndTime <= Reservation.StartTime)
            {
                ModelState.AddModelError("Invalid_EndTime", "EndTime cannot be earlier than StartTime");
                return Page();
            }

            if ((Reservation.StartTime - DateTime.Now.TimeOfDay) < TimeSpan.FromMinutes(15)
                && Reservation.ArrivalDate.Date == DateTime.Now.Date)
            {
                ModelState.AddModelError("MinimumStartTimeError", "Reservation must be created 15 minutes in advance");
                return Page();
            }

            if (Reservation.StartTime < TimeSpan.FromHours(7) || Reservation.EndTime > TimeSpan.FromHours(21))
            {
                ModelState.AddModelError("WorkingHoursError", "The store is open from 7AM - 9PM");
                return Page();
            }

            Reservation.CustomerId = HttpContext.Session.GetInt32("_Id");

            SelectedTables.Clear();

            string[] selectedTableIds = Request.Form["selectedTable"];

            foreach (var tableIdStr in selectedTableIds)
            {
                if (int.TryParse(tableIdStr, out int tableId))
                {
                    SelectedTables.Add(tableId);
                }
            }

            if (SelectedTables.Count > 0)
            {
                Reservation.CreateTime = DateTime.Now;
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
                return RedirectToPage("/MomoPages/MomoInfo", new { id = Reservation.ReservationId });
            }
            else
            {
                Reservation.Status = 1;
                _reservationRepo.Update(Reservation);
            }

            return RedirectToPage("/Homepage");
        }

        public IActionResult OnGetGetTable(DateTime arrivalDate, TimeSpan startTime, TimeSpan endTime)
        {
            var tableList = CheckAvailableTable(arrivalDate, startTime, endTime);

            if (tableList != null && tableList.Count() > 0)
            {
                var areaList = _areaRepo.GetAll();

                foreach (var table in tableList)
                {
                    table.Area = areaList.FirstOrDefault(al => al.AreaId == table.AreaId);
                }
            }

            var result = JsonSerializer.Serialize(tableList, new JsonSerializerOptions());

            return Content(result, "application/json");
        }

        private List<Table> CheckAvailableTable(DateTime arrivalDate, TimeSpan startTime, TimeSpan endTime)
        {
            var availableTables = _tableRepo.GetAll();
            var reservationOfTheDay = new List<Reservation>();

            foreach (var reservation in _reservationRepo.GetAll())
            {
                if (reservation.ArrivalDate.Date.Equals(arrivalDate) && (reservation.Status == 1))
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
                    var reservationStartTime = reservation.StartTime;
                    var reservationEndTime = reservation.EndTime;

                    var requestedStartTime = startTime;
                    var requestedEndTime = endTime;

                    if (!(requestedStartTime > reservationEndTime || requestedEndTime < reservationStartTime))
                    {
                        var overlapsedReservation = ReservationTables.Where(rt => rt.ReservationId == reservation.ReservationId);

                        foreach (var reservationTable in overlapsedReservation)
                        {
                            availableTables.RemoveAll(t => t.TableId == reservationTable.TableId);
                        }
                    }
                }

                availableTables.RemoveAll(t => t.Status == 0);
            }

            return availableTables;
        }
    }
}
