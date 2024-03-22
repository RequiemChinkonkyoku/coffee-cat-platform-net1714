using CoffeeCatPlatform.Pages.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Repositories;

namespace CoffeeCatPlatform.Pages.ManagerPages
{
    public class ReservationManagementModel : ManagerAuthModel
    {
        private readonly IRepositoryBase<Reservation> _reservationRepo;
        private readonly IRepositoryBase<Customer> _customerRepo;

        public List<Reservation> ReservationList { get; set; }
        public List<Customer> CustomerList { get; set; }

        public ReservationManagementModel(IRepositoryBase<Reservation> reservationRepo, IRepositoryBase<Customer> customerRepo)
        {
            _reservationRepo = reservationRepo;
            _customerRepo = customerRepo;

            ReservationList = new List<Reservation>();
            CustomerList = new List<Customer>();
        }


        public IActionResult OnGet()
        {
            IActionResult auth = ManagerAuthorize();
            if (auth != null)
            {
                return auth;
            }

            var reservationList = _reservationRepo.GetAll();
            var customerList = _customerRepo.GetAll();

            if (reservationList.Count() > 0)
            {
                foreach (var reservation in reservationList)
                {
                    var customer = customerList.FirstOrDefault(x => x.CustomerId == reservation.CustomerId);
                    if (customer != null)
                    {
                        reservation.Customer = customer;
                    }
                    ReservationList.Add(reservation);
                }
            }

            return Page();
        }
    }
}
