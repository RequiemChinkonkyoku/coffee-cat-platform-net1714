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
    public class ViewReservationModel : CustomerAuthModel
    {
        private readonly IRepositoryBase<Reservation> _reservationRepo;
        private readonly IRepositoryBase<Customer> _customerRepo;
        public List<Reservation> ReservationList { get; set; }

        public ViewReservationModel(IRepositoryBase<Reservation> reservationRepo, IRepositoryBase<Customer> customerRepo)
        {
            _reservationRepo = reservationRepo;
            _customerRepo = customerRepo;

            ReservationList = new List<Reservation>();
        }

        public IActionResult OnGet()
        {
            IActionResult auth = CustomerAuthorize();
            if (auth != null)
            {
                return auth;
            }
            var customerId = HttpContext.Session.GetInt32("_Id");

            var reservationList = _reservationRepo.GetAll().Where(r => r.CustomerId == customerId);

            if (reservationList.Count() > 0)
            {
                foreach (var reservation in reservationList)
                {
                    reservation.Customer = _customerRepo.GetAll().FirstOrDefault(c => c.CustomerId == customerId);
                    ReservationList.Add(reservation);
                }
            }

            return Page();
        }
    }
}
