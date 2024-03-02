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

        public List<BillProduct> BillProducts { get; set; }
        public List<Bill> Bills { get; set; }
        public List<Product> Products { get; set; }

        public string SelectedPromotionName { get; set; }
        public Bill Bill { get; set; }

        public DetailsModel(
            IRepositoryBase<BillProduct> billProductRepository,
            IRepositoryBase<Bill> billRepository,
            IRepositoryBase<Product> productRepository,
            IRepositoryBase<Promotion> promotionRepository)
        {
            _billProductRepository = billProductRepository;
            _billRepository = billRepository;
            _productRepository = productRepository;

            _promotionRepository = promotionRepository;
            Bill = new Bill();

            BillProducts = new List<BillProduct>();
            Bills = new List<Bill>();
            Products = new List<Product>();
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
