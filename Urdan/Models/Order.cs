using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Urdan.Models
{
	public class Order
	{
		[Key]
		public int Id { get; set; }
		public int UserId { get; set; }
		public int ShippingAddressId { get; set; }
		public string Status { get; set; } = "Ongoing deliveries";
		public string PaymentMethod { get; set; } = "Cash on delivery";
		[Precision(18, 2)]
		public decimal ShippingFee { get; set; }
		[Precision(18, 2)]
		public decimal Total { get; set; } = 0;
		public DateTime CreatedAt { get; set; } = DateTime.Now;
		[Column(TypeName = "Date")]
		[DataType(DataType.Date)]
		[DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
		public DateTime DeliveryDate { get; set; } = DateTime.Now.AddDays(3);
		public virtual User? User { get; set; }
		public virtual ShippingAddress ShippingAddress { get; set; }
		public virtual ICollection<OrderItem>? OrderItems { get; set; }
	}
}
