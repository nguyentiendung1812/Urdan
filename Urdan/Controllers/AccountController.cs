using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Urdan.Data;
using Urdan.Models;
using Urdan.Services;

namespace Urdan.Controllers
{
  public class AccountController : Controller
  {
    private readonly UrdanContext _context;
    private readonly IUserService _userService;

    public AccountController(UrdanContext context, IUserService userService)
    {
      _context = context;
      _userService = userService;
    }

    private bool IsLoggedIn()
    {
      return HttpContext.User.Identity?.IsAuthenticated ?? false;
    }

    // GET: /Account
    [Authorize()]
    public async Task<IActionResult> Index()
    {
      var username = HttpContext.User.Identity?.Name;
      var userList = HttpContext.User.Claims.ToList();
      Debug.WriteLine(userList);
      var user = await _userService.FirstOrDefaultAsync(u => u.Username == username);
      return View(user);
    }

    // GET: /Account/Addresses
    [Authorize()]
    public async Task<IActionResult> Addresses()
    {
      var username = HttpContext.User.Identity?.Name;
      List<Address> addresses = await _context.Addresses.Include(a => a.User).Where(a => a.User.Username == username).ToListAsync();
      return View(addresses);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteAddress(int id)
    {
      var address = await _context.Addresses.FirstOrDefaultAsync(a => a.Id == id);
      if (address != null)
      {
        _context.Remove(address);
        await _context.SaveChangesAsync();
      }

      return RedirectToAction(nameof(Addresses));
    }

    [Authorize()]
    // GET: /Account/AddAddress
    public async Task<IActionResult> AddAddress()
    {
      Address address = new Address();

      var username = HttpContext.User.Identity?.Name;
      var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
      if (user != null)
      {
        address.UserId = user.Id;
      }
      return View(address);
    }

    // POST: /Account/HandleAddAddress
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> HandleAddAddress(Address address)
    {
      if (ModelState.IsValid)
      {
        int addressCount = await _context.Addresses.CountAsync();
        if (addressCount == 0)
        {
          address.IsDefault = true;
        }
        else
        {
          address.IsDefault = false;
        }
        _context.Add(address);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Addresses));
      }
      return View();
    }

    public async Task<IActionResult> EditAddress()
    {
      var userId = User.Claims.FirstOrDefault(claim => claim.Type == "Id")?.Value;
      var address = await _context.Addresses.FirstOrDefaultAsync(a => a.UserId.ToString() == userId);
      return View(address);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> HandleEditAddress(Address address)
    {
      if (ModelState.IsValid)
      {
        try
        {
          _context.Update(address);
          await _context.SaveChangesAsync();
          return RedirectToAction(nameof(Addresses));
        }
        catch (DbUpdateConcurrencyException)
        {
          throw;
        }
      }

      return View(nameof(EditAddress));
    }

    // GET: /Account/Orders
    public async Task<IActionResult> Orders()
    {
      List<Order> orders = await _context.Orders.Include(o => o.User).Include(o => o.OrderItems).Include(o => o.ShippingAddress).ToListAsync();
      return View(orders);
    }

    // GET: /Account/Register
    public IActionResult Register()
    {
      bool isLoggedIn = IsLoggedIn();
      if (isLoggedIn)
      {
        return RedirectToAction(nameof(Index));
      }
      return View();
    }

    // GET: /Account/Login
    public IActionResult Login()
    {
      bool isLoggedIn = IsLoggedIn();
      if (isLoggedIn)
      {
        return RedirectToAction(nameof(Index));
      }
      return View();
    }


    // POST: /Account/HandleRegister 
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> HandleRegister(User userModel)
    {
      if (ModelState.IsValid)
      {
        var usernameExists = await _context.Users.FirstOrDefaultAsync(u => u.Username == userModel.Username);

        if (usernameExists != null)
        {
          ModelState.AddModelError("Username", "The username already exists");
          return View(nameof(Register));
        }

        var emailExists = await _context.Users.FirstOrDefaultAsync(u => u.Email == userModel.Email);
        if (emailExists != null)
        {
          ModelState.AddModelError("Email", "Email address already exists");
          return View(nameof(Register));
        }

        string salt = BC.GenerateSalt(10);
        string hashedPassword = BC.HashPassword(userModel.Password, salt);
        userModel.Password = hashedPassword;
        await _context.AddAsync(userModel);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Login));
      }

      return View(nameof(Register));
    }


    // POST: /Account/HandleLogin
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> HandleLogin(string Username, string Password)
    {
      if (ModelState.IsValid)
      {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == Username);

        if (user != null && BC.Verify(Password, user.Password))
        {
          var claims = new List<Claim>
          {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Email,user.Email),
            new Claim("Id",user.Id.ToString()),
            new Claim(ClaimTypes.Role,user.Role == Role.Admin ? "Admin" : "User")
          };
          var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);


          await HttpContext.SignInAsync(
               CookieAuthenticationDefaults.AuthenticationScheme,
               new ClaimsPrincipal(claimsIdentity));
          return RedirectToAction(nameof(Index));
        }
        else
        {
          ModelState.AddModelError("CustomError", "Invalid username or password");

        }


      }
      return View(nameof(Login));
    }


    // GET: /Account/Logout
    public async Task<IActionResult> Logout()
    {
      await HttpContext.SignOutAsync();
      return RedirectToAction(nameof(Login));
    }
  }
}
