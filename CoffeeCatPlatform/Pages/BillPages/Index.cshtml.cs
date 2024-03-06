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
        public List<Bill> Bills { get; set; }
        public List<Promotion> Promotions { get; set; }

        public IndexModel(IRepositoryBase<Bill> billRepository, IRepositoryBase<Promotion> promotionRepository)
        {
            _billRepository = billRepository;
            _promotionRepository = promotionRepository;
        }

        public IActionResult OnGet()
        {
            Bills = _billRepository.GetAll();
            Promotions = _promotionRepository.GetAll();
            
            foreach (var bill in Bills)
            {
                var promotion = Promotions.FirstOrDefault(p => p.PromotionId == bill.PromotionId);
                if (promotion != null)
                {
                    bill.Promotion = promotion;
                }
            }

            return Page();
        }
    }
}
