using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Urdan.Data;
using Urdan.Models;

namespace Urdan.ViewComponents
{
	[ViewComponent(Name = "Category")]

	public class CategoryViewComponent : ViewComponent
	{

		private readonly UrdanContext _context;
		public CategoryViewComponent(UrdanContext context)
		{
			_context = context;
		}
		public async Task<IViewComponentResult> InvokeAsync()
		{
			List<Category> categories = await _context.Categories.ToListAsync();
			return View("Index", categories);
		}

	}
}
