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

            var reservation = _reservationRepo.GetAll().FirstOrDefault(r => r.ReservationId == id);

            if (reservation != null)
            {
                Reservation = reservation;

                var currentDate = DateTime.Now.Date;

                if (DateTime.Compare(currentDate, Reservation.ArrivalDate.Date) >= 2)
                {
                    RefundAmount = Reservation.TotalPrice.ToString();
                }
                else
                {
                    RefundAmount = (Reservation.TotalPrice / 2).ToString();
                }
            }
            else
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPost(OrderInfoModel model, int reservationId)
        {
            var response = await _momoRepo.CreateRefundAsync(model, reservationId);

            if (response != null)
            {
                Console.Error.WriteLine(response.ErrorCode);
            }

            var reservation = _reservationRepo.FindById(reservationId);

            if (reservation != null)
            {
                Reservation = reservation;
            }

            Reservation.Status = 0;
            _reservationRepo.Update(Reservation);

            return RedirectToPage("/CustomerPages/ReservationPages/ViewReservation");
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
