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
    public class ReservationDetailModel : CustomerAuthModel
    {
        private readonly IRepositoryBase<Reservation> _reservationRepo;
        private readonly IRepositoryBase<Bill> _billRepo;
        private readonly IRepositoryBase<Product> _productRepo;
        private readonly IRepositoryBase<BillProduct> _billProductRepo;

        public Reservation Reservation { get; set; }
        public Bill Bill { get; set; }

        [BindProperty]
        public List<BillProduct> BillProducts { get; set; }

        public ReservationDetailModel(IRepositoryBase<Reservation> reservationRepo,
                                      IRepositoryBase<Bill> billRepo,
                                      IRepositoryBase<Product> productRepo,
                                      IRepositoryBase<BillProduct> billProductRepo)
        {
            _reservationRepo = reservationRepo;
            _billRepo = billRepo;
            _productRepo = productRepo;
            _billProductRepo = billProductRepo;

            BillProducts = new List<BillProduct>();
        }

        public IActionResult OnGet(int id)
        {
            IActionResult auth = CustomerAuthorize();
            if (auth != null)
            {
                return auth;
            }

            var reservation = _reservationRepo.FindById(id);

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
            }
            else
            {
                return NotFound();
            }

            return Page();
        }
    }
}
