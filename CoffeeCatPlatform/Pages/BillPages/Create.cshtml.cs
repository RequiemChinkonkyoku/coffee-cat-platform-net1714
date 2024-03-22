using CoffeeCatPlatform.Pages.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Repositories;

namespace CoffeeCatPlatform.Pages.BillPages
{
    public class CreateModel : StaffAuthModel
    {
        private readonly IRepositoryBase<Product> _productRepository;
        private readonly IRepositoryBase<Bill> _billRepository;
        private readonly IRepositoryBase<BillProduct> _billProductRepository;
        private readonly IRepositoryBase<Promotion> _promotionRepository;
        private readonly IRepositoryBase<Reservation> _reservationRepository;
        private readonly IRepositoryBase<Customer> _customerRepository;

        public List<Product> Products { get; set; }
        public List<Promotion> Promotions { get; set; }
        public List<Reservation> Reservations { get; set; }

        public CreateModel(
           IRepositoryBase<Product> productRepository,
           IRepositoryBase<Bill> billRepository,
           IRepositoryBase<BillProduct> billProductRepository,
           IRepositoryBase<Promotion> promotionRepository,
           IRepositoryBase<Reservation> reservationRepository,
           IRepositoryBase<Customer> customerRepository
           )
        {
            _productRepository = productRepository;
            _billRepository = billRepository;
            _billProductRepository = billProductRepository;
            _promotionRepository = promotionRepository;
            _reservationRepository = reservationRepository;
            _customerRepository = customerRepository;

            Products = new List<Product>();
            Promotions = new List<Promotion>();
            Reservations = new List<Reservation>();
        }

        public IActionResult OnGet()
        {
            Products = _productRepository.GetAll();
            Promotions = _promotionRepository.GetAll();
            Reservations = _reservationRepository.GetAll().Where(r => r.ArrivalDate == DateTime.Now.Date
                                                                 && r.Status == 1).ToList();

            foreach (var reservation in Reservations)
            {
                reservation.Customer = _customerRepository.GetAll()
                    .FirstOrDefault(c => c.CustomerId == reservation.CustomerId);
            }

            return Page();
        }
        public IActionResult OnPost(Dictionary<int, int> productQuantities, List<int> selectedProducts, string? note, int? promotionId, int? reservationId)
        {
            if (selectedProducts == null || selectedProducts.Count == 0)
            {
                ModelState.AddModelError("", "Please select at least one product!");
                return OnGet();
            }

            int? staffId = HttpContext.Session.GetInt32("_Id");

            if (reservationId == null)
            {
                ModelState.AddModelError("", "Must choose a reservation!");
                return OnGet();
            }

            // create a new Bill
            var newBill = new Bill
            {
                TotalPrice = 0,
                Status = 0,
                PaymentTime = DateTime.Now,
                PromotionId = promotionId,
                Note = note,
                StaffId = staffId,
                ReservationId = reservationId

            };

            _billRepository.Add(newBill);

            foreach (var productId in selectedProducts)
            {
                var quantity = productQuantities.ContainsKey(productId) ? productQuantities[productId] : 0;

                if (quantity > 0)
                {
                    var product = _productRepository.GetAll().FirstOrDefault(p => p.ProductId == productId);

                    if (product != null)
                    {
                        var newBillProduct = new BillProduct
                        {
                            BillId = newBill.BillId,
                            ProductId = productId,
                            Quantity = quantity
                        };

                        _billProductRepository.Add(newBillProduct);

                        newBill.TotalPrice += quantity * product.Price;

                        if (newBill.PromotionId.HasValue)
                        {
                            var promotion = _promotionRepository.GetAll().FirstOrDefault(p => p.PromotionId == newBill.PromotionId);

                            if (promotion != null)
                            {
                                if (promotion.PromotionType == 0)
                                {
                                    newBill.TotalPrice -= promotion.PromotionAmount;
                                }
                                else if (promotion?.PromotionType == 1)
                                {
                                    newBill.TotalPrice -= (newBill.TotalPrice * promotion.PromotionAmount / 100);
                                }
                            }
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Please decide the quantity of the products");
                    return OnGet();
                }
            }

            _billRepository.Update(newBill);

            return RedirectToPage("Index");
        }
    }
}
