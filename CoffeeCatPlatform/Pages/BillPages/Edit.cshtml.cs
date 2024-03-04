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

        public List<Product> Products { get; set; }
        public List<Promotion> Promotions { get; set; }
        public List<BillProduct> BillProducts { get; set; }
        public List<Bill> Bills { get; set; }

        public Bill Bill { get; set; }

        public List<int?> SelectedProducts { get; set; }
        public List<int?> SelectedPromotions { get; set; }

        public EditModel(
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
            BillProducts = new List<BillProduct>();
            Bills = new List<Bill>();

            Bill = new Bill();


            SelectedProducts = new List<int?>();
            SelectedPromotions = new List<int?>();
        }
        public IActionResult OnGet(int id)
        {
            Bill = _billRepository.GetAll().FirstOrDefault(b => b.BillId == id);

            Bills = _billRepository.GetAll();
            Products = _productRepository.GetAll();
            Promotions = _promotionRepository.GetAll();

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

        public IActionResult OnPost(int id, Dictionary<int, int> productQuantities, List<int> selectedProducts, string note, int? promotionId)
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

                // Manually load BillProducts for the existingBill
                existingBill.BillProducts = _billProductRepository
                    .GetAll()
                    .Where(bp => bp.BillId == existingBill.BillId)
                    .ToList();

                // Load Product data for each BillProduct
                foreach (var billProduct in existingBill.BillProducts)
                {
                    billProduct.Product = _productRepository.GetAll()
                        .FirstOrDefault(p => p.ProductId == billProduct.ProductId);
                }

                // Step 3: Update or add BillProducts based on selected products and remove unchecked products

                var productsToRemove = existingBill.BillProducts
                         .Where(bp => bp.ProductId.HasValue && selectedProducts.Contains(bp.ProductId.Value) == false)
                         .ToList();

                foreach (var productToRemove in productsToRemove)
                {
                    existingBill.BillProducts.Remove(productToRemove);
                    _billProductRepository.Delete(productToRemove); // Optional: Delete the BillProduct from the repository
                }


                foreach (var productId in selectedProducts)
                {
                    var quantity = productQuantities.ContainsKey(productId) ? productQuantities[productId] : 0;

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
                            billProduct = new BillProduct
                            {
                                BillId = existingBill.BillId,
                                ProductId = productId,
                                Quantity = quantity
                            };

                            // Make sure to initialize the BillProducts collection if it's null
                            existingBill.BillProducts ??= new List<BillProduct>();

                            existingBill.BillProducts.Add(billProduct);
                        }
                    }
                }

                // Step 4: Calculate the new TotalPrice
                if (existingBill.BillProducts != null)
                {
                    existingBill.TotalPrice = existingBill.BillProducts.Sum(bp => bp.Quantity * (bp.Product?.Price ?? 0));
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
