using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Models;
using Repositories;
using Repositories.Impl;

namespace CoffeeCatPlatform.Pages.BillPages
{
    public class DetailsModel : PageModel
    {
        private readonly IRepositoryBase<BillProduct> _billProductRepository;
        private readonly IRepositoryBase<Bill> _billRepository;
        private readonly IRepositoryBase<Product> _productRepository;
        private readonly IRepositoryBase<Promotion> _promotionRepository;
        private readonly IRepositoryBase<Staff> _staffRepository;
        private readonly IRepositoryBase<Reservation> _reservationRepository;
        private readonly IRepositoryBase<Customer> _customerRepository;

        public List<BillProduct> BillProducts { get; set; }
        public List<Bill> Bills { get; set; }
        public List<Product> Products { get; set; }

        public string SelectedPromotionName { get; set; }
        public Bill Bill { get; set; }
        public Promotion Promotion { get; set; }
        public Reservation Reservation { get; set; }
        public Staff Staff { get; set; }

        public DetailsModel(
            IRepositoryBase<BillProduct> billProductRepository,
            IRepositoryBase<Bill> billRepository,
            IRepositoryBase<Product> productRepository,
            IRepositoryBase<Promotion> promotionRepository,
            IRepositoryBase<Staff> staffRepository,
            IRepositoryBase<Reservation> reservationRepository,
            IRepositoryBase<Customer> customerRepository)
        {
            _billProductRepository = billProductRepository;
            _billRepository = billRepository;
            _productRepository = productRepository;

            _promotionRepository = promotionRepository;
            Bill = new Bill();
            Promotion = new Promotion();
            Reservation = new Reservation();

            BillProducts = new List<BillProduct>();
            Bills = new List<Bill>();
            Products = new List<Product>();
            _staffRepository = staffRepository;
            _reservationRepository = reservationRepository;
            _customerRepository = customerRepository;
        }

        public IActionResult OnGet(int id)
        {
            Bills = _billRepository.GetAll();
            Products = _productRepository.GetAll();


            foreach (BillProduct billProduct in _billProductRepository.GetAll())
            {
                if (billProduct.BillId == id)
                {
                    var bill = Bills.First(b => b.BillId == id);

                    if (bill != null)
                    {
                        billProduct.Bill = bill;
                    }

                    var product = Products.First(p => p.ProductId == billProduct.ProductId);

                    if (product != null)
                    {
                        billProduct.Product = product;	
                    }

                    BillProducts.Add(billProduct);
                }
            }

            Bill = _billRepository.GetAll().FirstOrDefault(b => b.BillId == id);

            Promotion = _promotionRepository.GetAll().FirstOrDefault(p => p.PromotionId == Bill.PromotionId);

            Reservation = _reservationRepository.GetAll().FirstOrDefault(r => r.ReservationId == Bill.ReservationId);
            if (Reservation != null)
            {
                Reservation.Customer = _customerRepository.GetAll().FirstOrDefault(c => c.CustomerId == Reservation.CustomerId);
            }

            Staff = _staffRepository.GetAll().FirstOrDefault(s => s.StaffId == Bill.StaffId);

            // Retrieve and set the selected promotion name
            SelectedPromotionName = _promotionRepository.GetAll()
                .Where(p => p.PromotionId == Bill?.PromotionId)
                .Select(p => p.Name)
                .FirstOrDefault();


            if (BillProducts == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}
