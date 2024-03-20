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

namespace CoffeeCatPlatform.Pages.CustomerPages.ReservationPages
{
    public class CancelReservationModel : CustomerAuthModel
    {
        private readonly IRepositoryBase<Reservation> _reservationRepo;

        [BindProperty]
        public Reservation Reservation { get; set; } = default!;

        public CancelReservationModel(IRepositoryBase<Reservation> reservationRepo)
        {
            _reservationRepo = reservationRepo;
        }

        public IActionResult OnGet(int? id)
        {
            IActionResult auth = CustomerAuthorize();
            if (auth != null)
            {
                return auth;
            }

            if (id == null || _reservationRepo.GetAll() == null)
            {
                return NotFound();
            }

            var reservation = _reservationRepo.GetAll().FirstOrDefault(m => m.ReservationId == id);

            if (reservation == null)
            {
                return NotFound();
            }
            else
            {
                Reservation = reservation;
            }
            return Page();
        }

        public IActionResult OnPost(int? id)
        {
            IActionResult auth = CustomerAuthorize();
            if (auth != null)
            {
                return auth;
            }

            if (id == null)
            {
                return NotFound();
            }
            var reservation = _reservationRepo.GetAll().FirstOrDefault(r => r.ReservationId == id);

            if (reservation != null)
            {
                Reservation = reservation;
            }

            if (Reservation.TotalPrice > 0 && Reservation.Status == 1)
            {
                return RedirectToPage("/MomoPages/MomoRefund", new { id = reservation.ReservationId, refundAmount = reservation.TotalPrice });
            }

            Reservation.Status = 0;
            _reservationRepo.Update(Reservation);

            return RedirectToPage("/CustomerPages/ReservationPages/ViewReservation");
        }
    }
}
