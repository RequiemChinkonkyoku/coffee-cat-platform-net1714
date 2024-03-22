using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Repositories;
using Repositories.Impl;

namespace CoffeeCatPlatform.Pages.MoMoPages
{
    public class MoMoInfo : PageModel
    {
        private readonly IMomoRepository _momoRepo;
        private readonly IRepositoryBase<Reservation> _reservationRepo;

        private const string SessionKeyName = "_Name";
        private const string SessionKeyId = "_Id";
        private const string SessionKeyType = "_Type";

        public Reservation Reservation { get; set; }

        [BindProperty]
        public string CustomerName { get; set; }

        public MoMoInfo(IMomoRepository momoRepo, IRepositoryBase<Reservation> reservationRepo)
        {
            _momoRepo = momoRepo;
            _reservationRepo = reservationRepo;
        }

        public IActionResult OnGet(int? id)
        {
            if (SessionCheck() == false)
            {
                return RedirectToPage("/ErrorPages/NotLoggedInError");
            }

            CustomerName = HttpContext.Session.GetString(SessionKeyName);

            if (id != null)
            {
                Reservation = _reservationRepo.FindById(id.Value);
            }
            else
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPost(OrderInfoModel model, int reservationID)
        {
            model.Amount = (double)_reservationRepo.FindById(reservationID).TotalPrice;

            var response = await _momoRepo.CreatePaymentAsync(model, reservationID);
            return Redirect(response.PayUrl);
        }

        private bool SessionCheck()
        {
            bool result = true;
            if (String.IsNullOrEmpty(HttpContext.Session.GetString(SessionKeyName))
                && String.IsNullOrEmpty(HttpContext.Session.GetString(SessionKeyId))
                && String.IsNullOrEmpty(HttpContext.Session.GetString(SessionKeyType)))
            {
                result = false;
            }
            return result;
        }
    }
}
