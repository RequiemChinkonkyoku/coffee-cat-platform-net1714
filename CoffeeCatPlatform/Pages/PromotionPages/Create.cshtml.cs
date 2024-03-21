using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Repositories;

namespace CoffeeCatPlatform.Pages.PromotionPages
{
	public class CreateModel : PageModel
	{
		private readonly IRepositoryBase<Promotion> _promotionRepository;

		public CreateModel(IRepositoryBase<Promotion> promotionRepository)
		{
			_promotionRepository = promotionRepository;
		}

		public IActionResult OnGet()
		{
			return Page();
		}

		[BindProperty]
		public Promotion Promotion { get; set; } = default!;


		public IActionResult OnPost()
		{
			if (!ModelState.IsValid)
			{
				return Page();
			}
			_promotionRepository.Add(Promotion);

			return Redirect("./Index");
		}
	}
}
