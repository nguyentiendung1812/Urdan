using System.ComponentModel.DataAnnotations;

namespace Urdan.Models
{
	public enum Role
	{
		[Display(Name = "User")]
		User,
		[Display(Name = "Admin")]
		Admin
	}
	public class User
	{
		[Key]
		public int Id { get; set; }
		[Required(ErrorMessage = "{0} is required")]
		[MinLength(2, ErrorMessage = "{0} must be at least 2 characters or more")]
		public string Username { get; set; }
		[Required(ErrorMessage = "{0} is required")]
		[EmailAddress]
		public string Email { get; set; }
		[Required(ErrorMessage = "{0} is required")]
		[MinLength(6, ErrorMessage = "{0} must be at least 6 characters or more")]
		[DataType(DataType.Password)]
		public string Password { get; set; }
		public Role Role { get; set; } = Role.User;
		public DateTime CreatedAt { get; set; } = DateTime.Now;
		public virtual ICollection<Address>? Addresses { get; set; }
	}
}
