namespace Billing.API.Models
{
    public class ItemModel
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public double SubTotal { get; set; }

    }
}