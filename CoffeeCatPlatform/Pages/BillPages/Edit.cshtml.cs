using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Repositories;
using System.Linq;

namespace CoffeeCatPlatform.Pages.BillPages
{
    public class EditModel : PageModel
    {
        private readonly IRepositoryBase<Product> _productRepository;
        private readonly IRepositoryBase<Bill> _billRepository;
        private readonly IRepositoryBase<BillProduct> _billProductRepository;
        private readonly IRepositoryBase<Promotion> _promotionRepository;
        private readonly IRepositoryBase<Staff> _staffRepository;
        private readonly IRepositoryBase<Reservation> _reservationRepository;
        private readonly IRepositoryBase<Customer> _customerRepository;


        public List<Product> Products { get; set; }
        public List<Promotion> Promotions { get; set; }
        public List<BillProduct> BillProducts { get; set; }
        public List<Bill> Bills { get; set; }
        public List<Reservation> Reservations { get; set; }

        public Bill Bill { get; set; }

        public List<int?> SelectedProducts { get; set; }
        public List<int?> SelectedPromotions { get; set; }
        public List<int?> SelectedReservations { get; set; }

        public EditModel(
            IRepositoryBase<Product> productRepository,
            IRepositoryBase<Bill> billRepository,
            IRepositoryBase<BillProduct> billProductRepository,
            IRepositoryBase<Promotion> promotionRepository,
            IRepositoryBase<Staff> staffRepository,
            IRepositoryBase<Reservation> reservationRepository,
            IRepositoryBase<Customer> customerRepository
        )
        {
            _productRepository = productRepository;
            _billRepository = billRepository;
            _billProductRepository = billProductRepository;
            _promotionRepository = promotionRepository;
            _staffRepository = staffRepository;
            _reservationRepository = reservationRepository;
            _customerRepository = customerRepository;

            Products = new List<Product>();
            Promotions = new List<Promotion>();
            BillProducts = new List<BillProduct>();
            Bills = new List<Bill>();
            Reservations = new List<Reservation>();

            Bill = new Bill();


            SelectedProducts = new List<int?>();
            SelectedPromotions = new List<int?>();
            SelectedReservations = new List<int?>();
        }
        public IActionResult OnGet(int id)
        {
            Bill = _billRepository.GetAll().FirstOrDefault(b => b.BillId == id);

            Bills = _billRepository.GetAll();
            Products = _productRepository.GetAll();
            Promotions = _promotionRepository.GetAll();
            Reservations = _reservationRepository.GetAll().Where(r => r.ArrivalDate == DateTime.Now.Date).ToList();

            SelectedProducts = _billProductRepository
                               .GetAll()
                               .Where(bp => bp.BillId == id)
                               .Select(bp => bp.ProductId)
                               .ToList();

            SelectedPromotions = _billRepository
                               .GetAll()
                               .Where(b => b.BillId == id)
                               .Select(b => b.PromotionId)
                               .ToList();
            
            SelectedReservations = _billRepository
                               .GetAll()
                               .Where(b => b.BillId == id)
                               .Select(b => b.ReservationId)
                               .ToList();

            foreach (var reservation in Reservations)
            {
                reservation.Customer = _customerRepository.GetAll()
                    .FirstOrDefault(c => c.CustomerId == reservation.CustomerId);
            }

            foreach (BillProduct billProduct in _billProductRepository.GetAll())
            {
                if (billProduct.BillId == id)
                {
                    var bill = Bills.First(b => b.BillId == id);

                    if (bill != null)
                    {
                        billProduct.Bill = bill;
                    }

                    BillProducts.Add(billProduct);
                }
            }

            if (BillProducts == null)
            {
                return NotFound();
            }

            return Page();
        }

        public IActionResult OnPost(int id, Dictionary<int, int> productQuantities, List<int> selectedProducts, string? note, int? promotionId, int? reservationId)
        {
            if (selectedProducts == null || selectedProducts.Count == 0)
            {
                ModelState.AddModelError("", "Please select at least one product!");
                return OnGet(id);
            }

            // Step 1: Retrieve the existing Bill from the database
            var existingBill = _billRepository.GetAll()
                .FirstOrDefault(b => b.BillId == id);


            if (existingBill != null)
            {
                // Step 2: Update properties of the existing Bill
                existingBill.Note = note;
                existingBill.PaymentTime = DateTime.Now;
                existingBill.PromotionId = promotionId;
                existingBill.ReservationId = reservationId;

                // Manually load BillProducts for the existingBill
                existingBill.BillProducts = _billProductRepository
                    .GetAll()
                    .Where(bp => bp.BillId == existingBill.BillId)
                    .ToList();

                // Step 3: Update or add BillProducts based on selected products and remove unchecked products

                var productsToRemove = existingBill.BillProducts
                         .Where(bp => bp.ProductId.HasValue && selectedProducts.Contains(bp.ProductId.Value) == false)
                         .ToList();

                foreach (var productToRemove in productsToRemove)
                {
                    existingBill.BillProducts.Remove(productToRemove);
                    _billProductRepository.Delete(productToRemove);
                }


                foreach (var productId in selectedProducts)
                {
                    var quantity = productQuantities.ContainsKey(productId) ? productQuantities[productId] : 0;

                    if (quantity > 0)
                    {
                        var billProduct = existingBill.BillProducts?.FirstOrDefault(bp => bp.ProductId == productId);

                        if (billProduct != null)
                        {
                            // Existing BillProduct, update the quantity
                            billProduct.Quantity = quantity;
                        }
                        else
                        {
                            // New BillProduct, add it to the Bill
                            var product = _productRepository.GetAll().FirstOrDefault(p => p.ProductId == productId);

                            if (product != null)
                            {
                                // Ensure that the BillProducts collection is loaded
                                existingBill.BillProducts = _billProductRepository
                                    .GetAll()
                                    .Where(bp => bp.BillId == existingBill.BillId)
                                    .ToList();

                                // Make sure to initialize the BillProducts collection if it's null
                                existingBill.BillProducts ??= new List<BillProduct>();

                                billProduct = new BillProduct
                                {
                                    BillId = existingBill.BillId,
                                    ProductId = productId,
                                    Quantity = quantity
                                };

                                existingBill.BillProducts.Add(billProduct);
                            }
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Please decide the quantity of the products");
                        return OnGet(id);
                    }
                }

                foreach (var billProduct in existingBill.BillProducts)
                {
                    billProduct.Product = _productRepository.GetAll()
                        .FirstOrDefault(p => p.ProductId == billProduct.ProductId);
                }

                // Step 4: Calculate the new TotalPrice
                if (existingBill.BillProducts != null)
                {
                    existingBill.TotalPrice = existingBill.BillProducts.Sum(bp => bp.Quantity * (bp.Product?.Price ?? 0));

                    if (existingBill.PromotionId.HasValue)
                    {
                        var promotion = _promotionRepository.GetAll().FirstOrDefault(p => p.PromotionId == existingBill.PromotionId);

                        if (promotion != null)
                        {
                            if (promotion.PromotionType == 0)
                            {
                                // Deduct fixed amount
                                existingBill.TotalPrice -= promotion.PromotionAmount;
                            }
                            else if (promotion?.PromotionType == 1)
                            {
                                // Deduct percentage
                                existingBill.TotalPrice -= (existingBill.TotalPrice * promotion.PromotionAmount / 100);
                            }
                        }
                    }
                }



                // Step 5: Update the existing Bill entity in the repository
                _billRepository.Update(existingBill);

                return RedirectToPage("Index");
            }

            // Handle the case where the existing Bill is not found
            ModelState.AddModelError("", "Bill not found.");
            return OnGet(Bill.BillId);
        }
    }
}
