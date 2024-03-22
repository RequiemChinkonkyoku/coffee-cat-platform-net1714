using CoffeeCatPlatform.Pages.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Repositories;

namespace CoffeeCatPlatform.Pages.ManagerPages
{
    public class ViewCustomerReservationDetailModel : ManagerAuthModel
    {
        private readonly IRepositoryBase<Reservation> _reservationRepo;
        private readonly IRepositoryBase<Bill> _billRepo;
        private readonly IRepositoryBase<Product> _productRepo;
        private readonly IRepositoryBase<BillProduct> _billProductRepo;
        private readonly IRepositoryBase<Customer> _customerRepo;

        public Reservation Reservation { get; set; }
        public Bill Bill { get; set; }

        [BindProperty]
        public List<BillProduct> BillProducts { get; set; }

        public List<Customer> CustomerList { get; set; }

        public ViewCustomerReservationDetailModel(IRepositoryBase<Reservation> reservationRepo,
                                                  IRepositoryBase<Bill> billRepo,
                                                  IRepositoryBase<Product> productRepo,
                                                  IRepositoryBase<BillProduct> billProductRepo,
                                                  IRepositoryBase<Customer> customerRepo)
        {
            _reservationRepo = reservationRepo;
            _billRepo = billRepo;
            _productRepo = productRepo;
            _billProductRepo = billProductRepo;
            _customerRepo = customerRepo;

            CustomerList = new List<Customer>();
            BillProducts = new List<BillProduct>();
        }


        public IActionResult OnGet(int id)
        {
            IActionResult auth = ManagerAuthorize();
            if (auth != null)
            {
                return auth;
            }

            var reservation = _reservationRepo.FindById(id);
            var customerList = _customerRepo.GetAll();

            if (reservation != null)
            {
                Reservation = reservation;

                var bill = _billRepo.GetAll().FirstOrDefault(b => b.ReservationId == id);

                if (bill != null)
                {
                    Bill = bill;

                    var billProducts = _billProductRepo.GetAll().Where(b => b.ProductId == bill.BillId);

                    if (billProducts.Count() > 0)
                    {
                        foreach (var item in billProducts)
                        {
                            var product = _productRepo.FindById(item.ProductId.Value);

                            item.Product = product;

                            BillProducts.Add(item);
                        }
                    }
                }
                var customer = customerList.FirstOrDefault(x => x.CustomerId == reservation.CustomerId);
                if (customer != null)
                {
                    reservation.Customer = customer;
                }
            }
            else
            {
                return NotFound();
            }

            return Page();
        }
    }
}
