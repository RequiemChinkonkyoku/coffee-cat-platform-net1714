using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;
using Models;
using Repositories;
using Repositories.Impl;

namespace CoffeeCatPlatform.Pages.AreaManagement
{
    public class ViewAreaModel : PageModel
    {
        private readonly IRepositoryBase<Area> _areaRepo;

        private readonly IRepositoryBase<AreaCat> _areaCatRepo;

        private readonly IRepositoryBase<Table> _tableRepo;

        public bool result = false;

        public ViewAreaModel() 
        {
            _areaRepo = new AreaRepository();
            _areaCatRepo = new AreaCatRepository();
            _tableRepo = new TableRepository();
        }

        public IList<Area> Areas { get; set; } = default!;

        public IList<AreaCat> AreaCats { get; set; } = default!;

        public IList<Table> Tables { get; set; } = default!;

        public void OnGet()
        {
            if (
               !_areaCatRepo.GetAll().IsNullOrEmpty() || 
                _areaRepo.GetAll().IsNullOrEmpty() 
                || _tableRepo.GetAll().IsNullOrEmpty()
                ) 
            {
                Areas = _areaRepo.GetAll();
                AreaCats = _areaCatRepo.GetAll();
                Tables = _tableRepo.GetAll();
                result = true;
            }
        }
    }
}
