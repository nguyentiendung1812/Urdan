using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Urdan.Data;
using Urdan.Models;
using Urdan.Services;
using X.PagedList;

namespace Urdan.Controllers
{

	[Authorize(Roles = "Admin")]
	public class AdminController : Controller
	{
		private readonly UrdanContext _context;
		private readonly IUserService _userService;
		private readonly IProductService _productService;
		private readonly int PageSize = 6;
		public AdminController(UrdanContext context, IUserService userService, IProductService productService)
		{
			_context = context;
			_userService = userService;
			_productService = productService;
		}


		// Admin Dashboard
		// GET: /Admin
		public IActionResult Index()
		{

			return View();
		}

		// Users

		// GET: /Admin/Users
		public async Task<IActionResult> Users(string sort, string search, int page = 1)
		{
			var users = _context.Users.AsEnumerable();

			// Search
			if (!String.IsNullOrWhiteSpace(search))
			{
				ViewBag.Search = search;
				users = users.Where(u => u.Username.ToLower().Contains(search.ToLower()));
			}


			// Sort
			switch (sort)
			{
				case "id_desc":
					users = users.OrderByDescending(u => u.Id);
					break;
				case "username_asc":
					users = users.OrderBy(u => u.Username);
					break;
				case "username_desc":
					users = users.OrderByDescending(u => u.Username);
					break;
				case "created_desc":
					users = users.OrderByDescending(u => u.CreatedAt);
					break;
				default:
					break;
			}

			ViewBag.IdSort = String.IsNullOrEmpty(sort) ? "id_desc" : "";
			ViewBag.UsernameSort = sort == "username_desc" ? "username_asc" : "username_desc";
			ViewBag.CreatedAtSort = String.IsNullOrEmpty(sort) ? "created_desc" : "";

			ViewBag.CurrentSort = sort;
			// Pagination
			users = await users.ToPagedListAsync(page, PageSize);

			return View(users);
		}

		// GET: /Admin/DetailsUser/[id]
		public async Task<IActionResult> DetailsUser(int id)
		{
			var user = await _userService.FirstOrDefaultAsync(u => u.Id == id);
			return View(user);
		}

		// GET: /Admin/EditUser/[id] 
		public async Task<IActionResult> EditUser(int id)
		{

			var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);


			if (user == null)
			{
				return NotFound();
			};


			return View(user);
		}

		// POST: /Admin/HandleEditUser
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> HandleEditUser(User user)
		{

			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(user);
					await _context.SaveChangesAsync();
					return RedirectToAction(nameof(Users));
				}
				catch (DbUpdateConcurrencyException)
				{
					throw;
				}
			}
			return View("EditUser", user);
		}

		// GET: /Admin/CreateUser
		public IActionResult CreateUser()
		{

			return View();
		}

		// POST: /Admin/HandleCreateUser 
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> HandleCreateUser(User userModel)
		{
			if (ModelState.IsValid)
			{
				var usernameExists = await _context.Users.FirstOrDefaultAsync(u => u.Username == userModel.Username);
				if (usernameExists != null)
				{
					ModelState.AddModelError("Username", "The username already exists");
					return View(nameof(CreateUser));
				}

				var emailExists = await _context.Users.FirstOrDefaultAsync(u => u.Email == userModel.Email);
				if (emailExists != null)
				{
					ModelState.AddModelError("Email", "Email address already exists");
					return View(nameof(CreateUser));
				}

				string salt = BC.GenerateSalt(10);
				string hashedPassword = BC.HashPassword(userModel.Password, salt);
				userModel.Password = hashedPassword;
				_context.Add(userModel);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Users));

			}
			return View(nameof(CreateUser));
		}

		// GET: /Admin/HandleDeleteUser/[id]
		public async Task<IActionResult> HandleDeleteUser(int id)
		{
			var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
			if (user != null)
			{
				_context.Remove(user);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Users));
			}
			return View(nameof(Users));
		}

		// Addresses

		// GET: /Admin/Addresses
		public async Task<IActionResult> Addresses()
		{
			List<Address> addresses = await _context.Addresses.Include(a => a.User).AsNoTracking().ToListAsync();
			return View(addresses);
		}

		public async Task<IActionResult> CreateAddress()
		{
			List<User> users = await _context.Users.ToListAsync();
			SelectList selectListUsers = new SelectList(users, "Id", "Username");
			ViewData["Users"] = selectListUsers;
			return View();
		}


		// POST: /Admin/HandleCreateAddress
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> HandleCreateAddress([Bind("Ward,District,City,Detail,UserId")] Address address)
		{
			if (ModelState.IsValid)
			{
				_context.Add(address);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Addresses));
			}

			List<User> users = await _context.Users.ToListAsync();
			SelectList selectListUsers = new SelectList(users, "Id", "Username");
			ViewData["Users"] = selectListUsers;
			return View(nameof(CreateAddress));
		}


		// GET: /Admin/HandleDeleteAddress
		public async Task<IActionResult> HandleDeleteAddress(int id)
		{
			var address = await _context.Addresses.FindAsync(id);
			if (address != null)
			{
				_context.Remove(address);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Addresses));
			}
			return View(nameof(Addresses));
		}

		// Products

		// GET: /Admin/Products
		public async Task<IActionResult> Products(string? search, string? sort, int page = 1)
		{
			IEnumerable<Product> products = _productService.AsEnumerable();

			if (!String.IsNullOrEmpty(search))
			{
				products = products.Where(p => p.Name.ToLower().Contains(search.ToLower()));
				ViewBag.Search = search;
			}

			switch (sort)
			{
				case "id_desc":
					products = products.OrderByDescending(p => p.Id);
					break;
				case "name_desc":
					products = products.OrderByDescending(p => p.Name);
					break;
				case "name_asc":
					products = products.OrderBy(p => p.Name);
					break;
				case "price_total_desc":
					products = products.OrderByDescending(p => p.PriceTotal);
					break;
				case "price_total_asc":
					products = products.OrderBy(p => p.PriceTotal);
					break;
				case "created_desc":
					products = products.OrderByDescending(p => p.CreatedAt);
					break;
				default:
					break;
			}

			ViewBag.IdSort = String.IsNullOrEmpty(sort) ? "id_desc" : "";
			ViewBag.NameSort = sort == "name_desc" ? "name_asc" : "name_desc";
			ViewBag.PriceTotalSort = sort == "price_total_desc" ? "price_total_asc" : "price_total_desc";
			ViewBag.CreatedAtSort = String.IsNullOrEmpty(sort) ? "created_desc" : "";

			if (!String.IsNullOrEmpty(sort))
			{
				ViewBag.CurrentSort = sort;
			}
			products = await products.ToPagedListAsync(page, PageSize);

			return View(products);
		}

		// GET: /Admin/DetailsProduct/[id]
		public async Task<IActionResult> DetailsProduct(int id)
		{
			var product = await _productService.FirstOrDefaultAsync(p => p.Id == id);
			return View(product);
		}


		// GET: /Admin/CreateProduct
		public async Task<IActionResult> CreateProduct()
		{
			var model = new Product();
			List<SelectListItem> brands = await _context.Brands.Select(b => new SelectListItem
			{
				Text = b.Name,
				Value = b.Id.ToString(),
			}).ToListAsync();
			List<SelectListItem> categories = await _context.Categories.Select(c => new SelectListItem
			{
				Text = c.Name,
				Value = c.Id.ToString(),
			}).ToListAsync();


			model.Brands = brands;
			model.Categories = categories;

			return View(model);
		}

		// POST: /Admin/HandleCreateProduct
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> HandleCreateProduct(Product product)
		{
			if (ModelState.IsValid)
			{
				_context.Add(product);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Products));
			}
			return View(nameof(CreateProduct));
		}


		// GET: /Admin/Brands
		public async Task<IActionResult> Brands()
		{
			List<Brand> brands = await _context.Brands.Include(b => b.Products).ToListAsync();
			return View(brands);
		}

		// GET: /Admin/CreateBrand
		public IActionResult CreateBrand()
		{
			return View();
		}


		// POST: /Admin/HandleCreateBrand
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> HandleCreateBrand(Brand brand)
		{
			if (ModelState.IsValid)
			{
				_context.Add(brand);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(CreateBrand));
			}
			return View(nameof(Brands));
		}


		public async Task<IActionResult> HandleDeleteBrand(int id)
		{
			var brand = await _context.Brands.FirstOrDefaultAsync(b => b.Id == id);

			if (brand != null)
			{
				_context.Remove(brand);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Brands));
			}

			return View(nameof(Brands));
		}

		// GET: /Admin/Orders
		public async Task<IActionResult> Orders(string? search, int page = 1)
		{
			IEnumerable<Order> orders = _context.Orders.Include(o => o.User).Include(o => o.ShippingAddress).AsNoTracking().AsEnumerable();
			if (!String.IsNullOrEmpty(search))
			{
				orders.Where(o => o.Id.ToString() == search);
				ViewBag.Search = search;
			}

			orders = await orders.ToPagedListAsync(page, PageSize);

			return View(orders);
		}

		// GET: /Admin/Images
		public async Task<IActionResult> Images()
		{
			List<Image> images = await _context.Images.Include(i => i.Product).AsNoTracking().ToListAsync();
			return View(images);
		}


		// GET: /Admin/CreateImage
		public async Task<IActionResult> CreateImage()
		{
			var model = new Image();
			List<Product> products = await _context.Products.ToListAsync();
			SelectList selectList = new SelectList(products, "Id", "Name");
			model.Products = selectList;
			return View(model);
		}

		// POST: /Admin/HandleCreateImage
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> HandleCreateImage(Image image, List<IFormFile> uploads)
		{
			if (uploads != null && uploads.Count > 0)
			{
				foreach (var upload in uploads)
				{

					var fileName = upload.FileName;
					var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/upload", fileName);
					using (var fileSrteam = new FileStream(filePath, FileMode.Create))
					{
						await upload.CopyToAsync(fileSrteam);
					}
					_context.Add(new Image { Url = Path.Combine("/upload", fileName), ProductId = image.ProductId });
				}
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Images));
			}
			return View(nameof(CreateImage));
		}

		// GET: /Admin/EditProduct/[id]
		public async Task<IActionResult> EditProduct(int id)
		{

			var product = await _context.Products.FindAsync(id);
			if (product == null)
			{
				return NotFound();
			}

			return View(product);
		}

		// POST: /Admin/HandleEditProduct
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> HandleEditProduct(Product productModel)
		{
			if (ModelState.IsValid)
			{
				var product = await _context.Products.FindAsync(productModel.Id);
				if (product != null)
				{
					product.Name = productModel.Name;
					product.Size = productModel.Size;
					product.Height = productModel.Height;
					product.Material = productModel.Material;
					product.Description = productModel.Description;
					product.Price = productModel.Price;
					product.Discount = productModel.Discount;
					try
					{
						_context.Update(product);
						await _context.SaveChangesAsync();
						return RedirectToAction(nameof(Products));
					}
					catch (DbUpdateException)
					{
						ModelState.AddModelError("", "Error when saving to the database");
					}

				}
			}

			return View(nameof(EditProduct));
		}

		// Colors

		// GET: /Admin/Colors
		public async Task<IActionResult> Colors(string search)
		{
			List<Color> colors = await _context.Colors.Include(c => c.Product).ToListAsync();
			if (!String.IsNullOrEmpty(search))
			{
				colors = colors.Where(c => c.ProductId == Int32.Parse(search)).ToList();
				ViewBag.Search = search;
			}
			return View(colors);
		}

		// GET: /Admin/CreateColor
		public async Task<IActionResult> CreateColor()
		{
			Color color = new Color();
			List<Product> products = await _context.Products.ToListAsync();
			SelectList selectList = new SelectList(products, "Id", "Name");
			color.Products = selectList;
			return View(color);
		}

		// POST: /Admin/HandleCreateColor
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> HandleCreateColor(Color color)
		{
			if (ModelState.IsValid)
			{
				_context.Add(color);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Colors));
			}

			return View(nameof(CreateColor));
		}

		// GET: /Admin/EditColor/[id]
		public async Task<IActionResult> EditColor(int id)
		{
			var color = await _context.Colors.FindAsync(id);
			if (color == null)
			{
				return NotFound();
			}

			return View(color);
		}

		// GET: /Admin/HandleDeleteColor/[id]
		public async Task<IActionResult> HandleDeleteColor(int id)
		{
			var color = await _context.Colors.FirstOrDefaultAsync(c => c.Id == id);
			if (color != null)
			{
				_context.Remove(color);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Colors));
			}

			return View(nameof(Colors));
		}





		// GET: /Admin/HandleDeleteImage/[id]
		public async Task<IActionResult> HandleDeleteImage(int id)
		{
			var image = await _context.Images.FirstOrDefaultAsync(i => i.Id == id);
			if (image != null)
			{
				_context.Remove(image);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Images));
			}
			return View(nameof(Images));
		}

		// GET: /Admin/HandleDeleteProduct/[id]
		public async Task<IActionResult> HandleDeleteProduct(int id)
		{
			var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
			if (product != null)
			{
				_context.Remove(product);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Products));
			}
			return View(nameof(Products));
		}



	}
}
