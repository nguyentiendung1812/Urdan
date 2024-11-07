using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Urdan.Data;
using Urdan.Models;
using Urdan.Services;

namespace Urdan.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly UrdanContext _context;
		private readonly IProductService _productService;

		public HomeController(ILogger<HomeController> logger, UrdanContext context, IProductService productService)
		{
			_logger = logger;
			_context = context;
			_productService = productService;
		}
		public async Task<IActionResult> Index()
		{
			return View(await _productService.GetAllAsync());
		}



		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}