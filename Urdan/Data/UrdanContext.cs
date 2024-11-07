using Microsoft.EntityFrameworkCore;
using Urdan.Models;

namespace Urdan.Data
{
	public class UrdanContext : DbContext
	{
		public UrdanContext(DbContextOptions<UrdanContext> options) : base(options) { }

		public DbSet<Product> Products { get; set; }
		public DbSet<Color> Colors { get; set; }
		public DbSet<Address> Addresses { get; set; }
		public DbSet<Rating> Ratings { get; set; }
		public DbSet<Brand> Brands { get; set; }
		public DbSet<Image> Images { get; set; }
		public DbSet<Category> Categories { get; set; }
		public DbSet<User> Users { get; set; }
		public DbSet<Order> Orders { get; set; }
		public DbSet<OrderItem> OrderItems { get; set; }
		public DbSet<ShippingAddress> ShippingAddresses { get; set; }

	}
}
