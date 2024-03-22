using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DAOs;
using Models;
using Repositories;
using Repositories.Impl;
using CoffeeCatPlatform.Pages.Shared;

namespace CoffeeCatPlatform.Pages.PromotionPages
{
    public class EditModel : ManagerAuthModel
    {
        private readonly IRepositoryBase<Promotion> _promotionRepository;

		public EditModel(IRepositoryBase<Promotion> promotionRepository)
		{
			_promotionRepository = promotionRepository;
		}

		[BindProperty]
        public Promotion Promotion { get; set; } = default!;

        public IActionResult OnGet(int id)
        {
            Promotion = _promotionRepository.GetAll().FirstOrDefault(p => p.PromotionId == id);

            if (Promotion == null)
            {
                TempData["Error"] = "Promotion not found!";
                return Redirect("./Index");
            }

            return Page();
        }


        public IActionResult OnPost(int id)
        {

			if (!ModelState.IsValid)
			{
				return Page();
			}

			var existingPromotion = _promotionRepository.GetAll().FirstOrDefault(c => c.PromotionId == id);

			if (existingPromotion == null)
			{
				TempData["Error"] = "Promotion not found!";
				return RedirectToPage("./Index");
			}

			existingPromotion.Name = Promotion.Name;
			existingPromotion.Description = Promotion.Description;
			existingPromotion.PromotionType = Promotion.PromotionType;
			existingPromotion.PromotionAmount = Promotion.PromotionAmount;

			_promotionRepository.Update(existingPromotion);

			TempData["Success"] = "Promotion updated successfully.";
			return RedirectToPage("./Index");
		}
    }
}
