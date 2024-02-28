using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Repositories;

namespace CoffeeCatPlatform.Pages.BillPages
{
    public class IndexModel : PageModel
    {
        private readonly IRepositoryBase<Bill> _billRepository;
        public List<Bill> Bills { get; set; }

        public IndexModel(IRepositoryBase<Bill> billRepository)
        {
            _billRepository = billRepository;
        }

        public IActionResult OnGet()
        {
            Bills = _billRepository.GetAll();
            return Page();
        }
    }
}
