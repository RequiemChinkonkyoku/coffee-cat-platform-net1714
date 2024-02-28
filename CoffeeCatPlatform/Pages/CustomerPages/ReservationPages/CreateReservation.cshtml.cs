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

namespace CoffeeCatPlatform.Pages.CustomerPages.ReservationPages
{
    public class CreateReservationModel : PageModel
    {
        [BindProperty]
        public Reservation Reservation { get; set; } = default!;

        private readonly IRepositoryBase<Reservation> _reservationRepo;

        private const string SessionKeyName = "_Name";
        private const string SessionKeyId = "_Id";
        private const string SessionKeyType = "_Type";

        public CreateReservationModel(ReservationRepository reservationRepo)
        {
            _reservationRepo = reservationRepo;
        }

        public IActionResult OnGet()
        {
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
