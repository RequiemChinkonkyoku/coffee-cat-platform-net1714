using CoffeeCatPlatform.Pages.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Repositories;
using System.Diagnostics;

namespace CoffeeCatPlatform.Pages.ManagerPages
{
    public class DashboardModel : PageModel
    {
        private readonly IRepositoryBase<Reservation> _reservationRepo;
        private readonly IRepositoryBase<Customer> _customerRepo;
        private readonly IRepositoryBase<Bill> _billRepo;

        private const string SessionKeyName = "_Name";
        private const string SessionKeyId = "_Id";
        private const string SessionKeyType = "_Type";

        [BindProperty]
        public int todaySales { get; set; }

        [BindProperty]
        public int todayCustomers { get; set; }

        [BindProperty]
        public int monthlySales { get; set; }

        public DashboardModel(IRepositoryBase<Reservation> reservationRepo, IRepositoryBase<Customer> customerRepo, IRepositoryBase<Bill> billRepo)
        {
            _reservationRepo = reservationRepo;
            _customerRepo = customerRepo;
            _billRepo = billRepo;

            Reservations = new List<Reservation>();
        }

        public List<Reservation> Reservations { get; set; }

        public IActionResult OnGet()
        {
            if (SessionCheck() == false)
            {
                return RedirectToPage("/ErrorPages/NotLoggedInError");
            }
            else if (!(HttpContext.Session.GetString(SessionKeyType) == "Manager"))
            {
                Debug.WriteLine(HttpContext.Session.GetString(SessionKeyType));
                return RedirectToPage("/ErrorPages/NotAuthorizedError");
            }

            todaySales = 0;
            foreach (var reservation in _reservationRepo.GetAll())
            {
                if (reservation.BookingDay.Date.Equals(DateTime.Today.Date)
                    && (reservation.Status == 1 || reservation.Status == 2))
                {
                    todaySales += (int)reservation.TotalPrice;
                }
            }
            foreach (var bill in _billRepo.GetAll())
            {
                if (bill.PaymentTime.Date.Equals(DateTime.Today.Date)
                    && bill.Status == 1)
                {
                    todaySales += (int)bill.TotalPrice;
                }
            }

            todayCustomers = 0;
            foreach (var reservation in _reservationRepo.GetAll())
            {
                if (reservation.BookingDay.Date.Equals(DateTime.Today.Date)
                    && (reservation.Status == 1 || reservation.Status == 2))
                {
                    todayCustomers += 1;
                }
            }

            monthlySales = 0;
            foreach (var reservation in _reservationRepo.GetAll())
            {
                if (reservation.BookingDay.Month.Equals(DateTime.Today.Month)
                    && (reservation.Status == 1 || reservation.Status == 2))
                {
                    monthlySales += (int)reservation.TotalPrice;
                }
            }
            foreach (var bill in _billRepo.GetAll())
            {
                if (bill.PaymentTime.Month.Equals(DateTime.Today.Month)
                    && bill.Status == 1)
                {
                    monthlySales += (int)bill.TotalPrice;
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
