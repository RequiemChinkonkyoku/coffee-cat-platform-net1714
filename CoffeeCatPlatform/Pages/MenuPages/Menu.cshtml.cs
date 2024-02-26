using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DAOs;
using Models;
using Repositories;
using Repositories.Impl;
using Microsoft.IdentityModel.Tokens;

namespace CoffeeCatPlatform.Pages.MenuPages
{
    public class MenuModel : PageModel
    {
        private readonly IRepositoryBase<Product> _productRepo;

        public const string SessionKeyName = "_Name";

        public bool result = false;

        public MenuModel()
        {
            _productRepo = new ProductRepository();
        }

        public IList<Product> Products { get; set; } = default!;

        public void OnGet()
        {
            if (!_productRepo.GetAll().IsNullOrEmpty())
            {
                Products = _productRepo.GetAll();
                result = true;
            }
        }
    }
}
