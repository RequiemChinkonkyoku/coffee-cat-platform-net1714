using CoffeeCatPlatform.Pages.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Repositories;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace CoffeeCatPlatform.Pages.ManagerPages
{
    public class CatManagementModel : StaffAuthModel
    {
        private readonly IRepositoryBase<Cat> _catRepository;

        public CatManagementModel(IRepositoryBase<Cat> catRepository)
        {
            _catRepository = catRepository;
        }


        [BindProperty(SupportsGet = true)]
        public string SearchQuery { get; set; }
        public IList<Cat> Cats { get; set; }

        public IActionResult OnGet(string? searchQuery)
        {
            IActionResult auth = ManagerAuthorize();
            if (auth != null)
            {
                return auth;
            }

            SearchQuery = searchQuery;
            IEnumerable<Cat> query = _catRepository.GetAll();

            if (!string.IsNullOrEmpty(searchQuery))
            {
                query = query.Where(p => p.Name.Contains(searchQuery, StringComparison.OrdinalIgnoreCase));
            }
                       
            Cats = query.ToList();
            return Page();
        }
    }
}
