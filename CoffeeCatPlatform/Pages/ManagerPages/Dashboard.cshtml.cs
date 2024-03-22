using CoffeeCatPlatform.Pages.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Repositories;
using System.Diagnostics;

namespace CoffeeCatPlatform.Pages.ManagerPages
{
    public class DashboardModel : StaffAuthModel
    {
        private readonly IRepositoryBase<Reservation> _reservationRepo;
        private readonly IRepositoryBase<Customer> _customerRepo;
        private readonly IRepositoryBase<Bill> _billRepo;

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
            IActionResult auth = ManagerAuthorize();
            if (auth != null)
            {
                return auth;
            }

            todaySales = 0;
            foreach (var reservation in _reservationRepo.GetAll())
            {
                if (reservation.ArrivalDate.Date.Equals(DateTime.Today.Date)
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
                if (reservation.ArrivalDate.Date.Equals(DateTime.Today.Date)
                    && (reservation.Status == 1 || reservation.Status == 2))
                {
                    todayCustomers += 1;
                }
            }

            monthlySales = 0;
            foreach (var reservation in _reservationRepo.GetAll())
            {
                if (reservation.ArrivalDate.Month.Equals(DateTime.Today.Month)
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
    }
}
