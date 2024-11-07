using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Urdan.Models
{
	public class Image
	{
		[Key]
		public int Id { get; set; }
		public int ProductId { get; set; }
		public string Url { get; set; }
		[NotMapped]
		public SelectList? Products { get; set; }
		public virtual Product? Product { get; set; }
	}
}
