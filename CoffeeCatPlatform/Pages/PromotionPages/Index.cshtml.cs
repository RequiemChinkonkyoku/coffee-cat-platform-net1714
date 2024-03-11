using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Repositories;

namespace CoffeeCatPlatform.Pages.PromotionPages
{
	public class IndexModel : PageModel
    {
        private readonly IRepositoryBase<Promotion> _promotionRepository;
		public List<Promotion> Promotions { get; set; }


		public IndexModel(IRepositoryBase<Promotion> promotionRepository)
		{
			_promotionRepository = promotionRepository;

            Promotions = new List<Promotion>();
		}


        public IActionResult OnGet()
        {
            Promotions = _promotionRepository.GetAll();
            return Page();
        }

        public IActionResult OnPostDelete(int id)
        {
            var promotion = _promotionRepository.GetAll().FirstOrDefault(p => p.PromotionId == id);
            if (promotion == null)
            {
                return NotFound();
            }
            _promotionRepository.Delete(promotion);
            return RedirectToPage("./Index");
        }

        public IActionResult OnGetEdit(int id)
        {
			return RedirectToPage("./Edit", new { id });
		}
    }
}
