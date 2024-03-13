using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Models;
using Repositories;
using Repositories.Impl;

namespace CoffeeCatPlatform.Pages.MoMoPages
{
    public class MoMoInfo : PageModel
    {
        private readonly IMomoRepository _momoRepo;
        private readonly IRepositoryBase<Reservation> _reservationRepo;
        public Reservation Reservation { get; set; }

        public MoMoInfo(IMomoRepository momoRepo, IRepositoryBase<Reservation> reservationRepo)
        {
            _momoRepo = momoRepo;
            _reservationRepo = reservationRepo;
        }

        public IActionResult OnGet()
        {
            var reservationID = int.Parse(TempData["ReservationID"].ToString());

            if (reservationID != null)
            {
                Reservation = _reservationRepo.FindById(reservationID);
            }

            return Page();
        }

        public async Task<IActionResult> OnPost(OrderInfoModel model, int reservationID)
        {
            var response = await _momoRepo.CreatePaymentAsync(model, reservationID);
            return Redirect(response.PayUrl);
        }
    }
}
