using Microsoft.AspNetCore.Mvc;

namespace Urdan.Controllers
{
	public class WishlistController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
