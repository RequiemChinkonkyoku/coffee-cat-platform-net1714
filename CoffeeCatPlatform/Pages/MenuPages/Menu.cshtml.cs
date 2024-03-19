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

		[Range(0, double.MaxValue, ErrorMessage = "Min Price must be a non-negative value.")]
		[RegularExpression(@"^\d*\.?\d*$", ErrorMessage = "Please enter a valid numeric value.")]
		[BindProperty(SupportsGet = true)]	
		public decimal MinPrice { get; set; }

		[Range(0, double.MaxValue, ErrorMessage = "Max Price must be a non-negative value.")]
		[RegularExpression(@"^\d*\.?\d*$", ErrorMessage = "Please enter a valid numeric value.")]
		[BindProperty(SupportsGet = true)]
		public decimal MaxPrice { get; set; }

		[BindProperty(SupportsGet = true)]
		public string SortByPrice { get; set; }

		[BindProperty(SupportsGet = true)]
		public string SortByName { get; set; }
		public IList<Product> Products { get; set; } = default!;

		public int TotalPages => (int)Math.Ceiling((double)TotalItems / ItemsPerPage);

		public IActionResult OnGet(string? currentPage, string? searchQuery, decimal minPrice, decimal maxPrice, string sortByPrice, string sortByName)
		{
			SearchQuery = searchQuery;
			MinPrice = minPrice;
			MaxPrice = maxPrice;
			SortByPrice = sortByPrice;
			SortByName = sortByName;

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
			if (sortByPrice == "asc")
			{
				query = query.OrderBy(p => p.Price);
			}
			else if (sortByPrice == "desc")
			{
				query = query.OrderByDescending(p => p.Price);
			}

			if (sortByName == "asc")
			{
				query = query.OrderBy(p => p.Name);
			}
			else if (sortByName == "desc")
			{
				query = query.OrderByDescending(p => p.Name);
			}

			TotalItems = query.Count();

			Products = query.Skip((CurrentPage - 1) * ItemsPerPage)
							.Take(ItemsPerPage)
							.ToList();

			

			return Page();
		}
	}
}
