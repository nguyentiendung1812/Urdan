using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Urdan.Data;
using Urdan.Models;
using Urdan.Services;
using X.PagedList;

namespace Urdan.Controllers
{
	public class ProductsController : Controller
	{
		private readonly UrdanContext _context;
		private readonly IProductService _productService;
		private readonly int _pageSize = 3;

		public ProductsController(UrdanContext context, IProductService productService)
		{
			_context = context;
			_productService = productService;
		}

		public async Task<IActionResult> Index(string? category, string? search, string? price, string? brand, string? sort, int page = 1)
		{
			var products = _productService.AsEnumerable();
			
			if (!String.IsNullOrEmpty(category))
			{
				products = products.Where(p => p.Category?.Name == category);
				ViewBag.CurrentCategory = category;
			}

			if (!String.IsNullOrEmpty(brand))
			{
				products = products.Where(p => p.Brand?.Name == brand);
				ViewBag.CurrentBrand = brand;
			}

			/*if (!String.IsNullOrEmpty(price))
			{
				String[] prices = price.Split(new char[] { ' ', '-' });
				int min = Int32.Parse(prices[0].Substring(1));
				int max = Int32.Parse(prices[prices.Length - 1].Substring(1));
				products = products.Where(p => p.PriceTotal >= min && p.PriceTotal <= max);
				ViewBag.PriceFilter = price;
			}*/



			if (!String.IsNullOrEmpty(search))
			{
				products = products.Where(p => p.Name.ToLower().Contains(search.ToLower()));
				ViewBag.ProductSearch = search;
			}


			switch (sort)
			{
				case "name_asc":
					products = products.OrderBy(p => p.Name);
					break;
				case "name_desc":
					products = products.OrderByDescending(p => p.Name);
					break;
				case "price_asc":
					products = products.OrderBy(p => p.PriceTotal);
					break;
				case "price_desc":
					products = products.OrderByDescending(p => p.PriceTotal);
					break;
				case "created_desc":
					products = products.OrderByDescending(p => p.CreatedAt);
					break;
				default: break;
			}


			ViewBag.CurrentSort = sort;

			var selectListItems = new List<SelectListItem> {
				new SelectListItem {Text = "Default Sorting",Value = ""},
				new SelectListItem {Text = "Sort by ascending name", Value = "name_asc"},
				new SelectListItem {Text = "Sort by descending name", Value = "name_desc"},
				new SelectListItem {Text = "Sort by ascending price", Value = "price_asc"},
				new SelectListItem {Text = "Sort by descending price", Value = "price_desc"},
				new SelectListItem {Text = "Sort by lastest products", Value = "created_desc"}
			};

			var selected = selectListItems.FindIndex(s => s.Value == sort);
			if (selected != -1)
			{
				selectListItems[selected].Selected = true;
			}
			ViewBag.SelectListSort = selectListItems;

			products = await products.ToPagedListAsync(page, _pageSize);

			ViewBag.Brands = await _context.Brands.Include(b => b.Products).AsNoTracking().ToListAsync();
			ViewBag.Colors = await _context.Colors.Select(c => c.Name).Distinct().ToListAsync();

			return View(products);
		}
	}

}
