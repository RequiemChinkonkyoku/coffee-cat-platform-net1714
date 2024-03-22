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
    public class ReservationDetailModel : PageModel
    {
        private readonly IRepositoryBase<Reservation> _reservationRepo;
        private readonly IRepositoryBase<Bill> _billRepo;
        private readonly IRepositoryBase<Product> _productRepo;
        private readonly IRepositoryBase<BillProduct> _billProductRepo;

        public Reservation Reservation { get; set; }
        public Bill Bill { get; set; }

        [BindProperty]
        public List<BillProduct> BillProducts { get; set; }

        private const string SessionKeyName = "_Name";
        private const string SessionKeyId = "_Id";
        private const string SessionKeyType = "_Type";

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
            if (SessionCheck() == false)
            {
                return RedirectToPage("/ErrorPages/NotLoggedInError");
            }

            var reservation = _reservationRepo.FindById(id);

            if (reservation != null)
            {
                Reservation = reservation;

                var bill = _billRepo.GetAll().FirstOrDefault(b => b.ReservationId == id);

                if (bill != null)
                {
                    Bill = bill;

                    var billProducts = _billProductRepo.GetAll().Where(bp => bp.BillId == bill.BillId);

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

        private bool SessionCheck()
        {
            bool result = true;
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(SessionKeyName))
                && string.IsNullOrEmpty(HttpContext.Session.GetString(SessionKeyId))
                && string.IsNullOrEmpty(HttpContext.Session.GetString(SessionKeyType)))
            {
                result = false;
            }
            return result;
        }
    }
}
