using Microsoft.AspNetCore.Mvc;

namespace Urdan.Controllers
{
	public class ContactController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
