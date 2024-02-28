using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Repositories;
using System.Text.RegularExpressions;

namespace CoffeeCatPlatform.Pages.MoMoPages
{
    public class PaymentCallBackModel : PageModel
    {
        private readonly IMomoRepository _momoRepo;
        private readonly IRepositoryBase<Reservation> _reservationRepo;
        public MomoExecuteResponseModel response { get; set; }

        public PaymentCallBackModel(IMomoRepository momoRepo, IRepositoryBase<Reservation> reservationRepo)
        {
            _momoRepo = momoRepo;
            _reservationRepo = reservationRepo;
        }

        public IActionResult OnGet()
        {
            response = _momoRepo.PaymentExecuteAsync(HttpContext.Request.Query);

            string pattern = @"ReservationID:\s*(\d+)";

            Match match = Regex.Match(response.OrderInfo, pattern);

            if (response.ErrorCode.Equals("0"))
            {
                string reservationIDString = match.Groups[1].Value;
                int reservationID = int.Parse(reservationIDString);
                CreateReservation(reservationID);
            }

            return Page();
        }

        private bool CreateReservation(int reservationID)
        {
            bool result = false;

            var reservation = _reservationRepo.FindById(reservationID);

            if (reservation != null)
            {
                reservation.Status = 1;
                _reservationRepo.Update(reservation);
            }

            return result;
        }
    }
}
