namespace Urdan.Models
{
	public class CartItem
	{
		public Guid Id { get; set; } = Guid.NewGuid();
		public int ProductId { get; set; }
		public string ProductName { get; set; }
		public string Image { get; set; }
		public string Color { get; set; }
		public decimal Price { get; set; }
		public int Quantity { get; set; }
		public decimal SubTotal
		{
			get
			{
				return this.Price * this.Quantity;
			}
		}
	}
}
