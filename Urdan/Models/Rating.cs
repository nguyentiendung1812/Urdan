using System.ComponentModel.DataAnnotations;

namespace Urdan.Models
{
	public class Rating
	{
		[Key]
		public int Id { get; set; }
		public int ProductId { get; set; }
		public int UserId { get; set; }
		[Range(1, 5)]
		public int Star { get; set; }
		public string Content { get; set; }
		[DataType(DataType.Date)]
		[DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
		public DateTime CreatedAt { get; set; } = DateTime.Now;
		public virtual User? User { get; set; }
		public virtual Product? Product { get; set; }
	}
}
