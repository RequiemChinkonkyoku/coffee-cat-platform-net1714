using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Repositories;

namespace CoffeeCatPlatform.Pages.MomoPages
{
    public class MomoRefundModel : PageModel
    {
        private readonly IMomoRepository _momoRepo;
        private readonly IRepositoryBase<Reservation> _reservationRepo;

        private const string SessionKeyName = "_Name";
        private const string SessionKeyId = "_Id";
        private const string SessionKeyType = "_Type";

        public Reservation Reservation { get; set; }

        [BindProperty]
        public string CustomerName { get; set; }

        [BindProperty]
        public string RefundAmount { get; set; }

        public MomoRefundModel(IMomoRepository momoRepo, IRepositoryBase<Reservation> reservationRepo)
        {
            _momoRepo = momoRepo;
            _reservationRepo = reservationRepo;
        }

        public IActionResult OnGet(int id)
        {
            if (SessionCheck() == false)
            {
                return RedirectToPage("/ErrorPages/NotLoggedInError");
            }

            CustomerName = HttpContext.Session.GetString(SessionKeyName);

            RefundAmount = GetRefundAmount(id).ToString();

            return Page();
        }

        public async Task<IActionResult> OnPost(OrderInfoModel model, int reservationId)
        {
            if (SessionCheck() == false)
            {
                return RedirectToPage("/ErrorPages/NotLoggedInError");
            }

            model.Amount = GetRefundAmount(reservationId);

            await _momoRepo.CreateRefundAsync(model, reservationId);

            Reservation.Status = 0;
            _reservationRepo.Update(Reservation);

            return RedirectToPage("/CustomerPages/ReservationPages/ViewReservation");
        }

        private double GetRefundAmount(int id)
        {
            double refundAmount = 0;
            var reservation = _reservationRepo.FindById(id);

            if (reservation != null)
            {
                Reservation = reservation;

                var currentDate = DateTime.Now.Date;

                if (DateTime.Compare(currentDate, Reservation.ArrivalDate.Date) >= 2)
                {
                    refundAmount = (double)Reservation.TotalPrice;
                }
                else
                {
                    refundAmount = (double)((Reservation.TotalPrice) / 2);
                }
            }

            return refundAmount;
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
