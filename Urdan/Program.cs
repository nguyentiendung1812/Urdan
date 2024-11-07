using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Urdan.Data;
using Urdan.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to database.
builder.Services.AddDbContext<UrdanContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("UrdanContext")));

builder.Services.AddSession(options =>
{
	options.Cookie.HttpOnly = true;
});


// Add services to the container.
builder.Services.AddControllersWithViews();


// Services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IProductService, ProductService>();


builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
				.AddCookie(options =>
				{
					options.LoginPath = "/Account/Login";
				});



var app = builder.Build();

app.UseSession();

//Seed datas to database
using (var scope = app.Services.CreateScope())
{
	var services = scope.ServiceProvider;
	SeedData.Seed(services);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
