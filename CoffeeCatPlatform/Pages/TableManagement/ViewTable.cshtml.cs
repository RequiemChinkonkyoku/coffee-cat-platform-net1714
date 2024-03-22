using CoffeeCatPlatform.Pages.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;
using Models;
using Repositories;
using Repositories.Impl;

namespace CoffeeCatPlatform.Pages.TableManagement
{
    public class ViewTableModel : ManagerAuthModel
    {
        private readonly IRepositoryBase<Table> _tableRepo;
        private readonly IRepositoryBase<Area> _areaRepo;
        public bool result = false;

        public ViewTableModel()
        {
            _tableRepo = new TableRepository();
            _areaRepo = new AreaRepository();
        }

        public IList<Table> Tables { get; set; } = default!;

        public void OnGet()
        {
            if (!_tableRepo.GetAll().IsNullOrEmpty())
            {
                Tables = _tableRepo.GetAll();
                foreach (var table in Tables)
                {
                    table.Area = _areaRepo.GetAll().FirstOrDefault(x => x.AreaId == table.AreaId);
                }
                result = true;
            }
        }
    }
}
