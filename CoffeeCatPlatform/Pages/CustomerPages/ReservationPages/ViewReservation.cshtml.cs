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

namespace CoffeeCatPlatform.Pages.CustomerPages.ReservationPages
{
    public class ViewReservationModel : PageModel
    {
        private readonly IRepositoryBase<Reservation> _reservationRepo;
        private readonly IRepositoryBase<Customer> _customerRepo;
        public List<Reservation> ReservationList { get; set; }

        private const string SessionKeyName = "_Name";
        private const string SessionKeyId = "_Id";
        private const string SessionKeyType = "_Type";

        public ViewReservationModel(IRepositoryBase<Reservation> reservationRepo, IRepositoryBase<Customer> customerRepo)
        {
            _reservationRepo = reservationRepo;
            _customerRepo = customerRepo;

            ReservationList = new List<Reservation>();
        }

        public IActionResult OnGet()
        {
            if (SessionCheck() == false)
            {
                return RedirectToPage("/ErrorPages/NotLoggedInError");
            }

            var customerId = HttpContext.Session.GetInt32(SessionKeyId);

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
