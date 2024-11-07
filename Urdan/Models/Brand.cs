using System.ComponentModel.DataAnnotations;

namespace Urdan.Models
{
	public class Brand
	{
		[Key]
		public int Id { get; set; }
		public string Name { get; set; }
		public virtual ICollection<Product>? Products { get; set; }
	}
}
