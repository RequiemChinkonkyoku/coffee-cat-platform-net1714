using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;
using Models;
using Repositories;
using Repositories.Impl;

namespace CoffeeCatPlatform.Pages.TableManagement
{
    public class ViewTableModel : PageModel
    {
        private readonly IRepositoryBase<Table> _tableRepo;
        public bool result = false;

        public ViewTableModel() 
        {
            _tableRepo = new TableRepository();
        }

        public IList<Table> Tables { get; set; } = default!;

        public void OnGet()
        {
            if (!_tableRepo.GetAll().IsNullOrEmpty()) 
            {
                Tables = _tableRepo.GetAll();
                result = true;
            }
        }
    }
}
