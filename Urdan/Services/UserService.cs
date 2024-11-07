using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Urdan.Data;
using Urdan.Models;

namespace Urdan.Services
{
	public interface IUserService
	{
		Task<User?> FirstOrDefaultAsync(Expression<Func<User, bool>> predicate);
		bool IsAdmin(int id);

	}

	public class UserService : IUserService
	{
		private readonly UrdanContext _context;
		public UserService(UrdanContext context)
		{
			_context = context;
		}

		public async Task<User?> FirstOrDefaultAsync(Expression<Func<User, bool>> predicate)
		{
			User? user = await _context.Users.Include(u => u.Addresses).AsNoTracking().FirstOrDefaultAsync(predicate);
			return user;
		}

		public bool IsAdmin(int id)
		{
			var user = _context.Users.Find(id);
			if (user == null) return false;
			if (user.Role == Role.Admin)
			{
				return true;
			}
			return false;
		}


	}
}
