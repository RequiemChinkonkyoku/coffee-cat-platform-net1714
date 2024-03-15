using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Repositories;

namespace CoffeeCatPlatform.Pages.BillPages
{
    public class IndexModel : PageModel
    {
        private readonly IRepositoryBase<Bill> _billRepository;
        private readonly IRepositoryBase<Promotion> _promotionRepository;
        private readonly IRepositoryBase<Reservation> _reservationRepository;
        private readonly IRepositoryBase<Staff> _staffRepository;
        public List<Bill> Bills { get; set; }
        public List<Promotion> Promotions { get; set; }
        public List<Reservation> Reservations { get; set; }
        public List<Staff> Staffs { get; set; }

        public IndexModel(IRepositoryBase<Bill> billRepository, IRepositoryBase<Promotion> promotionRepository, IRepositoryBase<Reservation> reservationRepository, IRepositoryBase<Staff> staffRepository)
        {
            _billRepository = billRepository;
            _promotionRepository = promotionRepository;
            _reservationRepository = reservationRepository;
            _staffRepository = staffRepository;
        }

        public IActionResult OnGet()
        {
            Bills = _billRepository.GetAll();
            Promotions = _promotionRepository.GetAll();
            Reservations = _reservationRepository.GetAll();
            Staffs = _staffRepository.GetAll();
            
            foreach (var bill in Bills)
            {
                var promotion = Promotions.FirstOrDefault(p => p.PromotionId == bill.PromotionId);
                if (promotion != null)
                {
                    bill.Promotion = promotion;
                }
                
                var reservation = Reservations.FirstOrDefault(p => p.ReservationId == bill.ReservationId);
                if (reservation != null)
                {
                    bill.Reservation = reservation;
                }
                var staff = Staffs.FirstOrDefault(p => p.StaffId == bill.StaffId);
                if (staff != null)
                {
                    bill.Staff = staff;
                }
            }

            return Page();
        }
    }
}
