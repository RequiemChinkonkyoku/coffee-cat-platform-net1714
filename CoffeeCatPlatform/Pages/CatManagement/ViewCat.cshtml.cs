using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Repositories;

namespace CoffeeCatPlatform.Pages.CatManagement
{
    public class CatViewModel : PageModel
    {
        private readonly IRepositoryBase<Cat> _catRepository;

        [BindProperty]
        public int NumberOfRows { get; set; }

        [BindProperty]
        public int NumberOfCats { get; set; }

        [BindProperty]
        public bool StaffLogin { get; set; }

        public CatViewModel(IRepositoryBase<Cat> catRepository)
        {
            _catRepository = catRepository;
        }

        public List<Cat> Cats { get; set; }

        public IActionResult OnGet()
        {
            StaffLogin = false;
            if (SessionCheck() == true)
            {
                if (HttpContext.Session.GetString(SessionKeyType).Equals("Staff"))
                {
                    StaffLogin = true;
                }
            }

            // Retrieve all cats from the repository for viewing
            Cats = _catRepository.GetAll();

            double temp = Cats.Count;
            NumberOfCats = (int)temp;
            NumberOfRows = (int)Math.Ceiling(temp / 4);

            // Perform any additional operations if needed
            return Page();
        }

        private const string SessionKeyName = "_Name";
        private const string SessionKeyId = "_Id";
        private const string SessionKeyType = "_Type";

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
