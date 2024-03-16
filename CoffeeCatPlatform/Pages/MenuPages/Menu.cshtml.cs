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
using Microsoft.Extensions.Configuration;
using System.ComponentModel.DataAnnotations;

namespace CoffeeCatPlatform.Pages.MenuPages
{
	public class MenuModel : PageModel
	{
		private readonly IRepositoryBase<Product> _productRepo;

		public bool result = false;

		public MenuModel()
		{
			_productRepo = new ProductRepository();
			Products = new List<Product>();
		}
		[BindProperty(SupportsGet = true)]

		public int CurrentPage { get; set; } = 1;
		public int TotalItems { get; set; }
		public int ItemsPerPage { get; set; } = 4;

		[BindProperty(SupportsGet = true)]
		public string SearchQuery { get; set; }

		[BindProperty(SupportsGet = true)]
		public decimal MinPrice { get; set; }

		[BindProperty(SupportsGet = true)]
		public decimal MaxPrice { get; set; }
		public IList<Product> Products { get; set; } = default!;

		public int TotalPages => (int)Math.Ceiling((double)TotalItems / ItemsPerPage);

		public IActionResult OnGet(string? currentPage, string? searchQuery, decimal minPrice, decimal maxPrice)
		{
			SearchQuery = searchQuery;
			MinPrice = minPrice;
			MaxPrice = maxPrice;

			if (int.TryParse(currentPage, out int temp))
			{
				CurrentPage = temp;
			}
			else
			{
				CurrentPage = 1;
			}

			IEnumerable<Product> query = _productRepo.GetAll();

			if (!string.IsNullOrEmpty(searchQuery))
			{
				query = query.Where(p => p.Name.Contains(searchQuery, StringComparison.OrdinalIgnoreCase));
			}

			if (MinPrice > 0 && MaxPrice > 0)
			{
				query = query.Where(p => p.Price >= MinPrice && p.Price <= MaxPrice);
			}

			TotalItems = query.Count();

			Products = query.Skip((CurrentPage - 1) * ItemsPerPage)
							.Take(ItemsPerPage)
							.ToList();

			result = Products.Count > 0;

			return Page();
		}
	}
}
