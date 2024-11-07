using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Urdan.Models
{
	public class OrderItem
	{
		[Key]
		public int Id { get; set; }
		public int OrderId { get; set; }
		public int ProductId { get; set; }
		public int Quantity { get; set; }
		[Precision(18, 2)]
		public decimal Price { get; set; }
		public string Color { get; set; }
		public virtual Product? Product { get; set; }
		public virtual Order? Order { get; set; }

	}
}
