using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Urdan.Data;
using Urdan.Extensions;
using Urdan.Models;
using Urdan.Services;

namespace Urdan.Controllers
{

	[Authorize()]
	public class CheckoutController : Controller
	{
		private readonly UrdanContext _context;
		private readonly IUserService _userService;
		public CheckoutController(UrdanContext context, IUserService userService)
		{
			_context = context;
			_userService = userService;
		}
		public async Task<IActionResult> Index()
		{
			var cart = HttpContext.Session.GetObject<Cart>("Cart");
			if (cart == null || cart.CartItems.Count == 0)
			{
				return RedirectToAction("Index", "Cart");
			}
			var userId = User.Claims.FirstOrDefault(claim => claim.Type == "Id")?.Value;

			Order order = new Order();
			Address? address = await _context.Addresses.FirstOrDefaultAsync(a => a.UserId.ToString() == userId);


			ViewBag.Address = address;
			ViewBag.Cart = cart;
			return View(order);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> CreateOrder(Order order)
		{

			if (ModelState.IsValid)
			{
				_context.Orders.Add(order);
				var cart = HttpContext.Session.GetObject<Cart>("Cart");
				foreach (var item in cart.CartItems)
				{
					OrderItem orderItem = new OrderItem { Order = order, ProductId = item.ProductId, Quantity = item.Quantity, Color = item.Color, Price = item.SubTotal };
					_context.OrderItems.Add(orderItem);
				}

				await _context.SaveChangesAsync();
				HttpContext.Session.Remove("Cart");
				return RedirectToAction("Orders", "Account");
			}



			return View(nameof(Index));
		}

	}
}
