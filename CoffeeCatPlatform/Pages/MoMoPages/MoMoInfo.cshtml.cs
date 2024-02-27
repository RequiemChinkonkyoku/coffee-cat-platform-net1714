using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Models;
using Repositories;

namespace CoffeeCatPlatform.Pages.MoMoPages
{
    public class MoMoInfo : PageModel
    {
        private readonly IMomoRepository _momoRepo;

        public MoMoInfo(IMomoRepository momoRepo)
        {
            _momoRepo = momoRepo;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPost(OrderInfoModel model)
        {
            var response = await _momoRepo.CreatePaymentAsync(model);
            return Redirect(response.PayUrl);
        }
    }
}
