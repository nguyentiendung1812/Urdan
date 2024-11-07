using Microsoft.AspNetCore.Mvc;
using Urdan.Data;
using Urdan.Extensions;
using Urdan.Models;
using Urdan.Services;

namespace Urdan.Controllers
{
	public class CartController : Controller
	{
		private readonly UrdanContext _context;
		private readonly IProductService _productService;
		public CartController(UrdanContext context, IProductService productService)
		{
			_context = context;
			_productService = productService;
		}
		public IActionResult Index()
		{
			Cart? cart = HttpContext.Session.GetObject<Cart>("Cart");
			return View(cart);
		}

		[HttpGet("/Cart/AddToCart/{productId}")]
		public async Task<IActionResult> AddToCart(int productId, string? color, string? quantityString)
		{
			int quantity = !String.IsNullOrEmpty(quantityString) ? Int32.Parse(quantityString) : 1;
			var product = await _productService.FirstOrDefaultAsync(p => p.Id == productId);
			if (product == null)
			{
				return NotFound();
			}

			color = color ?? product.Colors.First().Name;


			Cart cart = HttpContext.Session.GetObject<Cart>("Cart") ?? new Cart();
			int cartIndex = cart.CartItems.FindIndex(c => c.ProductId == productId && c.Color == color);
			if (cartIndex == -1)
			{
				cart.CartItems.Add(new CartItem { ProductId = productId, ProductName = product.Name, Image = product.Images.First().Url, Color = color, Price = product.PriceTotal, Quantity = quantity });
			}
			else
			{
				cart.CartItems[cartIndex].Quantity += quantity;
			}
			cart.Count += quantity;
			cart.PriceTotal += product.PriceTotal * quantity;


			HttpContext.Session.SetObject("Cart", cart);

			return RedirectToAction(nameof(Index));
		}


		public IActionResult UpdateCart(Guid id, string? quantityString)
		{
			Cart? cart = HttpContext.Session.GetObject<Cart>("Cart");
			if (cart != null && !String.IsNullOrEmpty(quantityString))
			{
				int quantity = Int32.Parse(quantityString);
				int cartIndex = cart.CartItems.FindIndex(c => c.Id == id);
				if (cartIndex != -1)
				{
					cart.Count -= cart.CartItems[cartIndex].Quantity;
					cart.PriceTotal -= cart.CartItems[cartIndex].SubTotal;
					cart.CartItems[cartIndex].Quantity = quantity;
					cart.Count += quantity;
					cart.PriceTotal += cart.CartItems[cartIndex].SubTotal;
					HttpContext.Session.SetObject("Cart", cart);
				}


			}
			return RedirectToAction(nameof(Index));
		}

		// GET: /Cart/DeleteCart/[id]
		public IActionResult DeleteCart(Guid id)
		{
			Cart? cart = HttpContext.Session.GetObject<Cart>("Cart");
			if (cart != null)
			{
				var cartItem = cart.CartItems.FirstOrDefault(c => c.Id == id);
				if (cartItem != null)
				{
					cart.Count -= cartItem.Quantity;
					cart.PriceTotal -= cartItem.SubTotal;
					cart.CartItems.Remove(cartItem);
					HttpContext.Session.SetObject("Cart", cart);
				}
				return RedirectToAction(nameof(Index));
			}

			return View(nameof(Index));
		}
	}
}
