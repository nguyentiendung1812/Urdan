using Microsoft.AspNetCore.Mvc;

namespace Urdan.Controllers
{
	public class AboutController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
