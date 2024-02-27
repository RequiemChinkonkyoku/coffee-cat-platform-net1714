using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Repositories;

namespace CoffeeCatPlatform.Pages.MoMoPages
{
    public class PaymentCallBackModel : PageModel
    {
        private readonly IMomoRepository _momoRepo;
        public MomoExecuteResponseModel response { get; set; }

        public PaymentCallBackModel(IMomoRepository momoRepo)
        {
            _momoRepo = momoRepo;
        }

        public IActionResult OnGet()
        {
            response = _momoRepo.PaymentExecuteAsync(HttpContext.Request.Query);
            return Page();
        }

    }
}
