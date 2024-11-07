using System.ComponentModel.DataAnnotations;

namespace Urdan.Models
{
	public class Address
	{
		[Key]
		public int Id { get; set; }
		public int UserId { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Ward { get; set; }
		public string District { get; set; }
		public string City { get; set; }
		public string Detail { get; set; }
		public string Phone { get; set; }
		public bool IsDefault { get; set; }
		public virtual User? User { get; set; }
		public string FullName { get { return $"{this.FirstName} {this.LastName}"; } }
	}
}
