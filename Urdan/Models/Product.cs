using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Urdan.Models
{
	public class Product
	{
		[Key]
		public int Id { get; set; }
		public int CategoryId { get; set; }
		public int BrandId { get; set; }
		public string Name { get; set; }
		public string Size { get; set; }
		public int Height { get; set; }
		public string Material { get; set; }
		public string Description { get; set; }
		[NotMapped]
		public List<SelectListItem> Brands { get; set; } = new List<SelectListItem>();
		[NotMapped]
		public List<SelectListItem> Categories { get; set; } = new List<SelectListItem>();
		[Precision(18, 2)]
		[DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
		public decimal Price { get; set; }
		[Range(0, 99)]
		public int Discount { get; set; } = 0;
		[Precision(18, 2)]
		[DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
		public decimal PriceTotal
		{
			get
			{
				if (Discount > 0)
				{
					return Price - (Price * Discount / 100);
				}
				else
				{
					return Price;
				}
			}
		}
		public decimal AvgRating
		{
			get
			{
				if (this.Ratings != null && this.Ratings.Count > 0)
				{
					int sumStar = 0;
					foreach (var s in this.Ratings)
					{
						sumStar += s.Star;
					}
					return sumStar / this.Ratings.Count;
				}
				else
				{
					return 0;
				}
			}
		}
		public DateTime CreatedAt { get; set; } = DateTime.Now;
		public virtual ICollection<Color>? Colors { get; set; }
		public virtual Category? Category { get; set; }
		public virtual Brand? Brand { get; set; }
		public virtual ICollection<Image>? Images { get; set; }
		public virtual ICollection<Rating>? Ratings { get; set; }
	}
}
