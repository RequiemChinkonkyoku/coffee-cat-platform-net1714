using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Repositories;

namespace CoffeeCatPlatform.Pages.BillPages
{
    public class CreateModel : PageModel
    {
        private readonly IRepositoryBase<Product> _productRepository;
        private readonly IRepositoryBase<Bill> _billRepository;
        private readonly IRepositoryBase<BillProduct> _billProductRepository;
        private readonly IRepositoryBase<Promotion> _promotionRepository;

        public List<Product> Products { get; set; }
        public List<Promotion> Promotions { get; set; }

        public CreateModel(
           IRepositoryBase<Product> productRepository,
           IRepositoryBase<Bill> billRepository,
           IRepositoryBase<BillProduct> billProductRepository,
           IRepositoryBase<Promotion> promotionRepository
           )
        {
            _productRepository = productRepository;
            _billRepository = billRepository;
            _billProductRepository = billProductRepository;
            _promotionRepository = promotionRepository;

            Products = new List<Product>();
            Promotions = new List<Promotion>();
        }

        public IActionResult OnGet()
        {
            // Retrieve all products for display
            Products = _productRepository.GetAll();
            Promotions = _promotionRepository.GetAll();
            return Page();
        }
        public IActionResult OnPost(Dictionary<int, int> productQuantities, List<int> selectedProducts, string note, int? promotionId)
        {
            if (selectedProducts == null || selectedProducts.Count == 0)
            {
                ModelState.AddModelError("", "Please select at least one product!");
                return OnGet();
            }

            // Step 1: Create a new Bill
            var newBill = new Bill
            {
                TotalPrice = 0,
                Status = 1,
                PaymentTime = DateTime.Now,
                PromotionId = promotionId,
                Note = note

                // Set other properties of the Bill if needed
            };

            _billRepository.Add(newBill);

            // Step 2: Create BillProducts based on selected products and link them to the new Bill
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

                        // Calculate the total price by summing the individual product prices
                        newBill.TotalPrice += quantity * product.Price;

                        if (newBill.PromotionId.HasValue)
                        {
                            var promotion = _promotionRepository.GetAll().FirstOrDefault(p => p.PromotionId == newBill.PromotionId);

                            if (promotion != null)
                            {
                                if (promotion.PromotionType == 0)
                                {
                                    // Deduct fixed amount
                                    newBill.TotalPrice -= promotion.PromotionAmount;
                                }
                                else if (promotion?.PromotionType == 1)
                                {
                                    // Deduct percentage
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

            // Update the newBill entity in the repository
            _billRepository.Update(newBill);

            return RedirectToPage("Index");
        }
    }
}
