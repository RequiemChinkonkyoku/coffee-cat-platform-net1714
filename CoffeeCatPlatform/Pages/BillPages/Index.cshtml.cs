using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Repositories;

namespace CoffeeCatPlatform.Pages.BillPages
{
    public class IndexModel : PageModel
    {
        private readonly IRepositoryBase<Bill> _billRepository;
        private readonly IRepositoryBase<Promotion> _promotionRepository;
        private readonly IRepositoryBase<Reservation> _reservationRepository;
        private readonly IRepositoryBase<Staff> _staffRepository;
        private readonly IRepositoryBase<Customer> _customerRepository;
        private readonly IRepositoryBase<BillProduct> _billProductRepository;
        private readonly IRepositoryBase<Product> _productRepository;
        public List<Bill> Bills { get; set; }
        public List<Promotion> Promotions { get; set; }
        public List<Reservation> Reservations { get; set; }
        public List<Staff> Staffs { get; set; }
        public List<int?> SelectedProducts { get; set; }


        public IndexModel(IRepositoryBase<Bill> billRepository, IRepositoryBase<Promotion> promotionRepository, IRepositoryBase<Reservation> reservationRepository, IRepositoryBase<Staff> staffRepository, IRepositoryBase<Customer> customerRepository, IRepositoryBase<BillProduct> billProductRepository, IRepositoryBase<Product> productRepository)
        {
            _billRepository = billRepository;
            _promotionRepository = promotionRepository;
            _reservationRepository = reservationRepository;
            _staffRepository = staffRepository;
            _customerRepository = customerRepository;

            Bills = new List<Bill>();
            SelectedProducts = new List<int?>();
            _billProductRepository = billProductRepository;
            _productRepository = productRepository;
        }

        public IActionResult OnGet()
        {
            Bills = _billRepository.GetAll();
            Promotions = _promotionRepository.GetAll();
            Reservations = _reservationRepository.GetAll();
            Staffs = _staffRepository.GetAll();

            foreach (var bill in Bills)
            {
                var promotion = Promotions.FirstOrDefault(p => p.PromotionId == bill.PromotionId);
                if (promotion != null)
                {
                    bill.Promotion = promotion;
                }

                var reservation = Reservations.FirstOrDefault(p => p.ReservationId == bill.ReservationId);
                if (reservation != null)
                {
                    bill.Reservation = reservation;
                    bill.Reservation.Customer = _customerRepository.GetAll().FirstOrDefault(c => c.CustomerId == bill.Reservation.CustomerId);
                }
                var staff = Staffs.FirstOrDefault(p => p.StaffId == bill.StaffId);
                if (staff != null)
                {
                    bill.Staff = staff;
                }
            }

            return Page();
        }

        public IActionResult OnPostCloseBill(int billId)
        {
            var bill = _billRepository.GetAll().FirstOrDefault(b => b.BillId == billId);

            if (bill == null)
            {
                return NotFound();
            }

            var billProducts = _billProductRepository.GetAll().Where(bp => bp.BillId == bill.BillId).ToList();

            foreach (var billProduct in billProducts)
            {
                var product = _productRepository.GetAll().FirstOrDefault(p => p.ProductId == billProduct.ProductId);

                if (product != null)
                {
                    product.Quantity -= billProduct.Quantity;

                    if (product.Quantity < 0)
                    {
                        product.Quantity = 0;
                    }

                    _productRepository.Update(product);
                }
            }

            bill.Status = 1;
            _billRepository.Update(bill);

            Reservations = _reservationRepository.GetAll();
            var reservation = Reservations.FirstOrDefault(p => p.ReservationId == bill.ReservationId);
            if (reservation != null)
            {
                reservation.Status = 2;
                _reservationRepository.Update(reservation);
            }

            return RedirectToPage("Index");
        }
    }
}
