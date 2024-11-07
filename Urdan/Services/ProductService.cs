using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Urdan.Data;
using Urdan.Models;

namespace Urdan.Services
{
	public interface IProductService
	{
		Task<List<Product>> GetAllAsync();
		IEnumerable<Product> AsEnumerable();

		Task<Product?> FirstOrDefaultAsync(Expression<Func<Product, bool>> predicate);
		Task<Product?> FindAsync(object keyValues);
	}

	public class ProductService : IProductService
	{
		private readonly UrdanContext _context;
		public ProductService(UrdanContext context)
		{
			_context = context;
		}


		public async Task<List<Product>> GetAllAsync()
		{
			List<Product> products = await _context.Products.Include(p => p.Category).Include(p => p.Brand).Include(p => p.Images).Include(p => p.Colors).AsNoTracking().ToListAsync();
			return products;
		}


		public async Task<Product?> FirstOrDefaultAsync(Expression<Func<Product, bool>> predicate)
		{
			var products = await _context.Products.Include(p => p.Category).Include(p => p.Brand).Include(p => p.Images).Include(p => p.Colors).Include(p => p.Ratings).ThenInclude(r => r.User).AsNoTracking().FirstOrDefaultAsync(predicate);
			return products;
		}

		public IEnumerable<Product> AsEnumerable()
		{
			return _context.Products.Include(p => p.Category).Include(p => p.Brand).Include(p => p.Images).Include(p => p.Colors).AsNoTracking().AsEnumerable();
		}

		public async Task<Product?> FindAsync(object keyValues)
		{
			var product = await _context.Products.FindAsync(keyValues);
			return product;
		}
	}
}
